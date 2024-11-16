using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.DependencyInjection;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using WebAPI.Filters;

namespace WebAPI;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers();
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddControllers( options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });
        builder.Services.InfrastructureServices();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.Run();
    }
}