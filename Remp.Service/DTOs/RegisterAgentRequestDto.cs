using System;

namespace Remp.Service.DTOs;

public class RegisterAgentRequestDto
{
  public string Email { get; set; }

  public string Password { get; set; }

  public string ConfirmPassword { get; set; }

  public string AgentFirstName { get; set; }
  public string AgentLastName { get; set; }

  public string? AvatarUrl { get; set; }

  public string? CompanyName { get; set; }
}
