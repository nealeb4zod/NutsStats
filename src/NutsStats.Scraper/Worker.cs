using Microsoft.EntityFrameworkCore;
using NutsStats.Infrastructure.Data;
using Microsoft.Extensions.Options;

namespace NutsStats.Scraper;

public sealed class Worker(
    ILogger<Worker> logger,
    ScraperDbContext dbContext,
    IOptions<ScraperOptions> options) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(Math.Max(1, options.Value.IntervalMinutes));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SeedSampleDataAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Scheduled scraper tick at {Time}", DateTimeOffset.UtcNow);

            try
            {
                await SeedSampleDataAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Scraper run failed");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task SeedSampleDataAsync(CancellationToken cancellationToken)
    {
        var existingVenues = await dbContext.Venues
            .AsNoTracking()
            .Select(v => v.VenueId)
            .ToListAsync(cancellationToken);

        if (existingVenues.Count > 0)
        {
            logger.LogInformation("Scraper found {Count} existing venues; no seed required.", existingVenues.Count);
            return;
        }

        var venue = new VenueEntity
        {
            VenueId = "sample-venue-1",
            VenueName = "Sample Venue",
            CreatedAt = DateTime.UtcNow,
            Events =
            [
                new EventEntity
                {
                    EventId = "sample-event-1",
                    EventUrl = "https://example.com/event/1",
                    Date = DateTime.UtcNow.AddDays(-1),
                    Results =
                    [
                        new ResultEntryEntity { Position = 1, PlayerName = "Alice", PlayerId = "alice", Points = 420 },
                        new ResultEntryEntity { Position = 2, PlayerName = "Bob", PlayerId = "bob", Points = 310 }
                    ]
                }
            ]
        };

        dbContext.Venues.Add(venue);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded sample venue data for {VenueName}", venue.VenueName);
    }
}

public sealed class ScraperOptions
{
    public int IntervalMinutes { get; set; } = 5;
}


