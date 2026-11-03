using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;
using Remp.Models.Entities;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<RempDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RempDb")));

builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(options =>
    options.User.RequireUniqueEmail = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<RempDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOpenApi();

var app = builder.Build();

// create user roles -admin/agent
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    if (!await roleManager.RoleExistsAsync("Agent"))
    {
        await roleManager.CreateAsync(new IdentityRole("Agent"));
    }
}

// Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();