using System;

namespace Remp.Models.Entities;

public class PhotographyCompany
{
  public string Id { get; set; }
  public string PhotographyCompanyName { get; set; }

  //Agent - photography company N:N
  public List<Agent> Agents { get; set; }
}
