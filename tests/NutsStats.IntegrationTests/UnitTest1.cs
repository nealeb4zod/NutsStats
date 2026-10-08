using Microsoft.EntityFrameworkCore;
using NutsStats.Application;
using NutsStats.Infrastructure;
using NutsStats.Infrastructure.Data;

namespace NutsStats.IntegrationTests;

public class VenueRepositoryIntegrationTests
{
    [Fact]
    public async Task GetVenuesAsync_ReturnsMappedVenueSummaries()
    {
        var options = new DbContextOptionsBuilder<ScraperDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ScraperDbContext(options);
        context.Venues.Add(new VenueEntity
        {
            VenueId = "venue-1",
            VenueName = "Test Venue",
            CreatedAt = DateTime.UtcNow,
            Events =
            [
                new EventEntity
                {
                    EventId = "event-1",
                    EventUrl = "https://example.test/event/1",
                    Date = DateTime.UtcNow.AddDays(-1),
                    Results =
                    [
                        new ResultEntryEntity { PlayerName = "Alice", PlayerId = "alice", Points = 100 }
                    ]
                }
            ]
        });
        await context.SaveChangesAsync();

        var repository = new VenueDataRepository(context);

        var result = await repository.GetVenuesAsync();

        var item = Assert.Single(result);
        Assert.Equal("venue-1", item.VenueId);
        Assert.Equal("Test Venue", item.VenueName);
        Assert.Equal(1, item.EventCount);
    }

    [Fact]
    public async Task GetVenueByIdAsync_ReturnsMappedDetail()
    {
        var options = new DbContextOptionsBuilder<ScraperDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ScraperDbContext(options);
        var venue = new VenueEntity
        {
            VenueId = "venue-2",
            VenueName = "Detail Venue",
            CreatedAt = DateTime.UtcNow,
            Events =
            [
                new EventEntity
                {
                    EventId = "event-2",
                    EventUrl = "https://example.test/event/2",
                    Date = DateTime.UtcNow.AddDays(-2),
                    Results =
                    [
                        new ResultEntryEntity { PlayerName = "Alice", PlayerId = "alice", Points = 200 },
                        new ResultEntryEntity { PlayerName = "Bob", PlayerId = "bob", Points = 150 }
                    ]
                }
            ]
        };
        context.Venues.Add(venue);
        await context.SaveChangesAsync();

        var repository = new VenueDataRepository(context);

        var result = await repository.GetVenueByIdAsync(venue.Id);

        Assert.NotNull(result);
        Assert.Equal("Detail Venue", result!.VenueName);
        Assert.Single(result.Events);
        Assert.Equal(2, result.Events[0].ResultCount);
    }
}
