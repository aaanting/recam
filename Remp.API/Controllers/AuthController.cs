using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Remp.Service.DTOs;
using Remp.Service.Interfaces;

namespace Remp.API.Controllers
{
    /// <summary>
    /// AUTH API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterRequestDto> _registerValidator;
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IValidator<RegisterAgentRequestDto> _registerAgentValidator;

        public AuthController(IAuthService authService, IValidator<RegisterRequestDto> registerValidator, IValidator<LoginRequestDto> loginValidator, IValidator<RegisterAgentRequestDto> registerAgentValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _registerAgentValidator = registerAgentValidator;
        }

        //Register new admin user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            var validation = await _registerValidator.ValidateAsync(registerDto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var result = await _authService.Register(registerDto);
            if (!result.Result.Succeeded)
            {
                return BadRequest(result.Result.Errors);
            }

            return StatusCode(StatusCodes.Status201Created, new
            {
                Message = "User Registration successful",
                Email = result.Email,
                Token = result.Token
            });
        }

        //Register new agent user
        [HttpPost("register-agent")]
        public async Task<IActionResult> RegisterAgent([FromBody] RegisterAgentRequestDto registerAgentDto)
        {
            var validation = await _registerAgentValidator.ValidateAsync(registerAgentDto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var result = await _authService.RegisterAgent(registerAgentDto);
            if (!result.Result.Succeeded)
            {
                return BadRequest(result.Result.Errors);
            }

            return StatusCode(StatusCodes.Status201Created, new
            {
                Message = "Agent Registration successful",
                Email = result.Email,
                Token = result.Token
            });
        }

        //login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var validation = await _loginValidator.ValidateAsync(loginDto);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var result = await _authService.Login(loginDto);
            if (result == null)
            {
                return Unauthorized("Login fail");
            }

            return Ok(result);
        }
    }
}
