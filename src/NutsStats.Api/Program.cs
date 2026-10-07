using NutsStats.Application;
using NutsStats.Infrastructure;
using NutsStats.Infrastructure.Data;
using Serilog;
using Microsoft.EntityFrameworkCore;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ??
            ["https://localhost:7188", "http://localhost:5099"];

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ScraperDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IVenueDataRepository, VenueDataRepository>();
builder.Services.AddScoped<IVenueStatsService, VenueStatsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("DefaultCors");
app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapGet("/api/venues", async (IVenueStatsService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.GetVenuesAsync(cancellationToken)))
    .WithName("GetVenues");

app.MapGet("/api/venues/{venueId:int}", async (int venueId, IVenueStatsService service, CancellationToken cancellationToken) =>
{
    var venue = await service.GetVenueByIdAsync(venueId, cancellationToken);
    return venue is null ? Results.NotFound() : Results.Ok(venue);
})
    .WithName("GetVenueById");

app.Run();

public partial class Program { }
