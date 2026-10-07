using Microsoft.EntityFrameworkCore;
using NutsStats.Infrastructure.Data;
using NutsStats.Scraper;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ScraperDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.Configure<ScraperOptions>(builder.Configuration.GetSection("Scraper"));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

public sealed class ScraperOptions
{
    public int IntervalMinutes { get; set; } = 5;
}
