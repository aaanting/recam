using System;

namespace Remp.Service.DTOs;

public class RegisterRequestDto
{
  public string Email { get; set; } = string.Empty;

  public string PhotographyCompanyName { get; set; } = string.Empty;

  public string Password { get; set; } = string.Empty;

  public string ConfirmPassword { get; set; } = string.Empty;
}
