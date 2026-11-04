using Remp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remp.DataAccess.Data;
using Remp.Service.DTOs;
using System.Security.Claims;
using FluentValidation;

namespace Remp.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly RempDbContext _dbContext;
        private readonly IAuthService _authService;
        private readonly IValidator<CreateAgentRequestDto> _validator;

        public AgentController(RempDbContext dbContext, IAuthService authService, IValidator<CreateAgentRequestDto> validator)
        {
            _dbContext = dbContext;
            _authService = authService;
            _validator = validator;
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAgent([FromBody] CreateAgentRequestDto createAgentRequestDto)
        {
            var validation = await _validator.ValidateAsync(createAgentRequestDto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Unauthorized();
            }
            var result = await _authService.RegisterAgent(createAgentRequestDto, adminId);

            if (!result.Result.Succeeded)
            {
                return BadRequest(result.Result.Errors);
            }

            return StatusCode(201, new
            {
                Message = "Agent created successfully",
                Email = result.Email
            });
        }
    }

}
