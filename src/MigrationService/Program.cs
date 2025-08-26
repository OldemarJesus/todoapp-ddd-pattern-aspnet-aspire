using Microsoft.EntityFrameworkCore;
using MigrationService;
using Web.Api.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

var host = builder.Build();
await host.RunAsync();
