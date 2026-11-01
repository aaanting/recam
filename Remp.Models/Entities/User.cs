using System;
using Microsoft.AspNetCore.Identity;

namespace Remp.Models.Entities;

public class User : IdentityUser
{
  public bool IsDeleted { get; set; }
  public DateTime CreatedAt { get; set; }

  //User --- ListingCase 1:N
  public List<ListingCase> ListingCases { get; set; }

  //User --- MediaAsset 1:N
  public List<MediaAsset> MediaAssets { get; set; }

}