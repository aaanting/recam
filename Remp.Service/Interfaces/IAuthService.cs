using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IAuthService
{
    Task<RegisterResultDto> Register(RegisterRequestDto registerDto);
    Task<RegisterResultDto> RegisterAgent(RegisterAgentRequestDto registerAgentDto);
    Task<LoginResponseDto?> Login(LoginRequestDto loginDto);
}
