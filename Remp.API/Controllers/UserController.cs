using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using Remp.Models.Entities;

namespace Remp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private RempDbContext _dbContext;
        public UserController(RempDbContext rempDb)
        {
            _dbContext = rempDb;
        }

        [HttpGet]
        public ActionResult GetAllUsers()
        {
            var users = _dbContext.Users.ToList();

            return Ok(users);
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
