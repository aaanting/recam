using Microsoft.EntityFrameworkCore;
using Remp.Common.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Remp.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RempDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, RempDbContext dbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        //Register admin
        public Task<RegisterResultDto> Register(RegisterRequestDto registerDto)
        {
            return RegisterUser(registerDto.Email, registerDto.Password, "Admin", user =>
            {
                var photographyCompany = new PhotographyCompany
                {
                    Id = user.Id,
                    PhotographyCompanyName = registerDto.PhotographyCompanyName?.Trim() ?? string.Empty
                };

                _dbContext.PhotographyCompanies.Add(photographyCompany);
            });
        }

        //Register agent
        public async Task<RegisterResultDto> RegisterAgent(CreateAgentRequestDto createAgentRequestDto, string adminId)
        {
            var photographyCompany = await _dbContext.PhotographyCompanies
                .FirstOrDefaultAsync(p => p.Id == adminId && !p.User.IsDeleted);

            if (photographyCompany == null)
            {
                return new RegisterResultDto
                {
                    Result = IdentityResult.Failed(new IdentityError
                    {
                        Code = "PhotographyCompanyNotFound",
                        Description = "Photography company not found."
                    })
                };
            }

            string password = PasswordHelper.GeneratePassword();

            var result = await RegisterUser(createAgentRequestDto.Email, password, "Agent", user =>
            {
                var agent = new Agent
                {
                    Id = user.Id,
                    AgentFirstName = createAgentRequestDto.AgentFirstName,
                    AgentLastName = createAgentRequestDto.AgentLastName,
                    AvatarUrl = createAgentRequestDto.AvatarUrl?.Trim() ?? string.Empty,
                    CompanyName = createAgentRequestDto.CompanyName?.Trim() ?? string.Empty,
                    PhotographyCompanies = new List<PhotographyCompany> { photographyCompany }
                };

                _dbContext.Agents.Add(agent);
            });

            return result;
        }

        private async Task<RegisterResultDto> RegisterUser(
            string email, string password, string role, Action<User> addProfile)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                var user = new User
                {
                    UserName = email,
                    Email = email
                };

                IdentityResult result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return new RegisterResultDto { Result = result };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    return new RegisterResultDto { Result = roleResult };
                }

                addProfile(user);
                await _dbContext.SaveChangesAsync();

                var roles = await _userManager.GetRolesAsync(user);
                string token = GenerateJwtToken(user, roles);

                await transaction.CommitAsync();

                return new RegisterResultDto
                {
                    Result = IdentityResult.Success,
                    Email = user.Email,
                    Token = token
                };
            }
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || user.IsDeleted)
            {
                return null;
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordValid)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new LoginResponseDto
            {
                Email = user.Email,
                Token = GenerateJwtToken(user, roles)
            };
        }

        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
