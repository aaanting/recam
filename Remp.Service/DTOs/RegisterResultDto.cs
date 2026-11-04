using Microsoft.AspNetCore.Identity;

namespace Remp.Service.DTOs;

public class RegisterResultDto
{
    public IdentityResult Result { get; set; } = IdentityResult.Success;
    public string? Email { get; set; }
    public string? Token { get; set; }
}
