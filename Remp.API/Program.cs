using Microsoft.EntityFrameworkCore;
using Remp.DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<RempDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RempDb")));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();