using System;

namespace Remp.Models.Entities;

public class Agent
{
  public string Id { get; set; }
  public string AgentFirstName { get; set; }
  public string AgentLastName { get; set; }
  public string AvatarUrl { get; set; }
  public string CompanyName { get; set; }

  // agent--listing case N:N
  public List<ListingCase> ListingCases { get; set; }

  // Agent--PhotographyCompany N:N
  public List<PhotographyCompany> PhotographyCompanies { get; set; }

}
