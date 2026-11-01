using System;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Remp.Models.Entities;

namespace Remp.DataAccess.Data;

public class RempDbContext : IdentityDbContext<User>
{

  public DbSet<ListingCase> ListingCases { get; set; }
  public DbSet<Agent> Agents { get; set; }
  public DbSet<CaseContact> CaseContacts { get; set; }
  public DbSet<MediaAsset> MediaAssets { get; set; }
  public DbSet<PhotographyCompany> PhotographyCompanies { get; set; }


  public RempDbContext(DbContextOptions<RempDbContext> options) : base(options)
  {
  }
}