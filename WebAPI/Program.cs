using Infrastructure.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.InfrastructureServices();

var app = builder.Build();

app.Run();
