using EnergyManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptionsAction: pgOptions =>
        {
            pgOptions.EnableRetryOnFailure(3);
            pgOptions.CommandTimeout(30);
        });
});

var app = builder.Build();



