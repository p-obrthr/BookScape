using System.ComponentModel.Design;
using Infrastructure.Data;
using LinqToDB.Common;
using LinqToDB.Common.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection InfrastructureServices(this IServiceCollection services)
    {
        var directory = GetDbDirectory();
        var connectionString = $"Data Source={directory}bookScape.db;";
        var appDbContext = new AppDbContext(connectionString);

        services.AddSingleton<AppDbContext>(provider => appDbContext);

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
