using System;

namespace Remp.Service.DTOs;

public class CreateAgentRequestDto
{
  public string Email { get; set; } = string.Empty;
  public string AgentFirstName { get; set; } = string.Empty;
  public string AgentLastName { get; set; } = string.Empty;
  public string? AvatarUrl { get; set; }
  public string? CompanyName { get; set; }
}
