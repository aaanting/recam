using Remp.Service.DTOs;

namespace Remp.Service.Interfaces;

public interface IAuthService
{
    Task<RegisterResultDto> Register(RegisterRequestDto registerDto);
    Task<LoginResponseDto?> Login(LoginRequestDto loginDto);
}
