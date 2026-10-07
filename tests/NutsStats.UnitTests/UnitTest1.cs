using NutsStats.Application;

namespace NutsStats.UnitTests;

public class VenueStatsServiceTests
{
    [Fact]
    public async Task GetVenuesAsync_DelegatesToRepository()
    {
        var expected = new[]
        {
            new VenueSummaryDto(1, "venue-1", "The Venue", 4, DateTime.UtcNow),
            new VenueSummaryDto(2, "venue-2", "Second Venue", 2, DateTime.UtcNow)
        };

        var repository = new StubVenueDataRepository(expected, null);
        var service = new VenueStatsService(repository);

        var result = await service.GetVenuesAsync();

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetVenueByIdAsync_DelegatesToRepository()
    {
        var expected = new VenueDetailDto(
            7,
            "venue-7",
            "Detail Venue",
            [new EventSummaryDto(9, "evt-9", "https://example.test/event", DateTime.UtcNow, 5)],
            DateTime.UtcNow);

        var repository = new StubVenueDataRepository([], expected);
        var service = new VenueStatsService(repository);

        var result = await service.GetVenueByIdAsync(7);

        Assert.Equal(expected, result);
    }

    private sealed class StubVenueDataRepository(
        IReadOnlyList<VenueSummaryDto> venues,
        VenueDetailDto? detail) : IVenueDataRepository
    {
        public Task<IReadOnlyList<VenueSummaryDto>> GetVenuesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(venues);

        public Task<VenueDetailDto?> GetVenueByIdAsync(int venueId, CancellationToken cancellationToken = default)
            => Task.FromResult(venueId == detail?.Id ? detail : null);
    }
}
