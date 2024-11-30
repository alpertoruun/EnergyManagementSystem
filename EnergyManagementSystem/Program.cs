using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Data.Context;
using EnergyManagementSystem.Data.Repositories;
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

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();



