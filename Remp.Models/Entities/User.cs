using System;
using Microsoft.AspNetCore.Identity;

namespace Remp.Models.Entities;

public class User : IdentityUser
{
  public bool IsDeleted { get; set; } = false;
  public DateTime CreatedAt { get; set; } = DateTime.Now;

  //User --- ListingCase 1:N
  public List<ListingCase> ListingCases { get; set; }

  //User --- MediaAsset 1:N
  public List<MediaAsset> MediaAssets { get; set; }

  //user -- PhotographyCompany 1:1
  public PhotographyCompany? PhotographyCompany { get; set; }

  //user -- agent 1:1
  public Agent? Agent { get; set; }

}