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

        public async Task<RegisterResultDto> Register(RegisterRequestDto registerDto)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                var user = new User
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    return new RegisterResultDto { Result = result };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded)
                {
                    return new RegisterResultDto { Result = roleResult };
                }

                var photographyCompany = new PhotographyCompany
                {
                    Id = user.Id,
                    PhotographyCompanyName = registerDto.PhotographyCompanyName
                };

                _dbContext.PhotographyCompanies.Add(photographyCompany);
                await _dbContext.SaveChangesAsync();

                // Generate before committing so a token generation failure rolls back registration.
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
