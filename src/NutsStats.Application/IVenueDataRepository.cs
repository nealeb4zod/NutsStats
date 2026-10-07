namespace NutsStats.Application;

public interface IVenueDataRepository
{
    Task<IReadOnlyList<VenueSummaryDto>> GetVenuesAsync(CancellationToken cancellationToken = default);
    Task<VenueDetailDto?> GetVenueByIdAsync(int venueId, CancellationToken cancellationToken = default);
}
