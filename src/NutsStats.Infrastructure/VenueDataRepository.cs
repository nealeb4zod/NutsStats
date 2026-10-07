using Microsoft.EntityFrameworkCore;
using NutsStats.Application;
using NutsStats.Infrastructure.Data;

namespace NutsStats.Infrastructure;

public sealed class VenueDataRepository(ScraperDbContext dbContext) : IVenueDataRepository
{
    public async Task<IReadOnlyList<VenueSummaryDto>> GetVenuesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Venues
            .AsNoTracking()
            .Select(v => new VenueSummaryDto(
                v.Id,
                v.VenueId,
                v.VenueName,
                v.Events.Count,
                v.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<VenueDetailDto?> GetVenueByIdAsync(int venueId, CancellationToken cancellationToken = default)
    {
        var venue = await dbContext.Venues
            .AsNoTracking()
            .Include(v => v.Events)
            .ThenInclude(e => e.Results)
            .FirstOrDefaultAsync(v => v.Id == venueId, cancellationToken);

        if (venue is null)
        {
            return null;
        }

        return new VenueDetailDto(
            venue.Id,
            venue.VenueId,
            venue.VenueName,
            venue.Events
                .Select(e => new EventSummaryDto(
                    e.Id,
                    e.EventId,
                    e.EventUrl,
                    e.Date,
                    e.Results.Count))
                .ToList(),
            venue.CreatedAt);
    }
}
