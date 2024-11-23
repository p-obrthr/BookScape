using Application.Services;
using Infrastructure.DependencyInjection;
using WebAPI.Filters;
using Microsoft.OpenApi.Models;

namespace WebAPI;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "BookScape Web API",
            });
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }   
                    }, Array.Empty<string>()
                }
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddControllers( options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });
        builder.Services.InfrastructureServices(builder.Configuration);

        
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT_KEY variable is not set.");
        }

        builder.Configuration["Jwt:Key"] = jwtKey;
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}