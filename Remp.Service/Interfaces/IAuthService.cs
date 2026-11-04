using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IAuthService
{
    Task<RegisterResultDto> Register(RegisterRequestDto registerDto);
    Task<RegisterResultDto> RegisterAgent(CreateAgentRequestDto createAgentRequestDto, string adminId);
    Task<LoginResponseDto?> Login(LoginRequestDto loginDto);
}
