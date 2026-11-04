using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using System.Security.Claims;

namespace Remp.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly RempDbContext _dbContext;

        public AgentController(RempDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("company-agents")]
        public IActionResult GetCompanyAgents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var agents = _dbContext.Agents.Where(a => a.PhotographyCompanies
                .Any(c => c.User.Id == userId)).ToList();
            return Ok(agents);
        }
    }
}
