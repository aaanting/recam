using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Remp.Service.DTOs;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private RempDbContext _dbContext;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UserController(RempDbContext rempDb, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = rempDb;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var users = _dbContext.Users.ToList();

            return Ok(users);
        }

        //Register new admin user
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = new User
                    {
                        UserName = userRegisterDto.Email,
                        Email = userRegisterDto.Email
                    };
                    var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
                    if (!result.Succeeded)
                    {
                        transaction.Rollback();
                        return BadRequest(result.Errors);
                    }

                    //assign admin
                    var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(roleResult.Errors);
                    }

                    var photographyCompany = new PhotographyCompany
                    {
                        Id = user.Id,
                        PhotographyCompanyName = userRegisterDto.PhotographyCompanyName
                    };

                    _dbContext.PhotographyCompanies.Add(photographyCompany);
                    _dbContext.SaveChanges();
                    transaction.Commit();

                    return Ok(new
                    {
                        Message = "User Registration successful",
                        UserEmail = user.Email
                    });
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        [HttpGet("search")]
        public ActionResult GetAgentByEmail([FromQuery] string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be empty.");
            }
            var agent = _dbContext.Agents.FirstOrDefault(a => a.User.Email == email);
            if (agent == null)
            {
                return NotFound("Agent not found");
            }
            return Ok(agent);
        }
    }
}
