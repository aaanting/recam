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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //constraints
        modelBuilder.Entity<Agent>().Property(p => p.AgentFirstName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Agent>().Property(p => p.AgentLastName).IsRequired().HasMaxLength(50);

        //Agent--user 1:1
        modelBuilder.Entity<Agent>().HasKey(a => a.Id);
        modelBuilder.Entity<Agent>().HasOne(a => a.User).WithOne(u => u.Agent)
                                    .HasForeignKey<Agent>(a => a.Id).OnDelete(DeleteBehavior.NoAction);
        //Agent--photgraphycompany 1:1
        modelBuilder.Entity<PhotographyCompany>().HasKey(p => p.Id);
        modelBuilder.Entity<PhotographyCompany>().HasOne(p => p.User).WithOne(u => u.PhotographyCompany)
            .HasForeignKey<PhotographyCompany>(p => p.Id).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<MediaAsset>().HasOne(m => m.User).WithMany(u => u.MediaAssets)
            .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.NoAction);
    }
}