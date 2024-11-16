using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using LinqToDB.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection InfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var directory = GetDbDirectory();
        var connectionString = $"Data Source={directory}bookScape.db;";
        var appDbContext = new AppDbContext(connectionString);

        services.AddSingleton<AppDbContext>(provider => appDbContext);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => 
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }

    public static string GetDbDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var  parentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        var dbDirectory = Path.Combine(parentDirectory ?? "", "Infrastructure", "Data") + "/";
        return dbDirectory;
    }
}