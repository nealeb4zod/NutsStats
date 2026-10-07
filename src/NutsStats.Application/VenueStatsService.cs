namespace NutsStats.Application;

public interface IVenueStatsService
{
    Task<IReadOnlyList<VenueSummaryDto>> GetVenuesAsync(CancellationToken cancellationToken = default);
    Task<VenueDetailDto?> GetVenueByIdAsync(int venueId, CancellationToken cancellationToken = default);
}

public sealed class VenueStatsService(IVenueDataRepository repository) : IVenueStatsService
{
    public Task<IReadOnlyList<VenueSummaryDto>> GetVenuesAsync(CancellationToken cancellationToken = default)
        => repository.GetVenuesAsync(cancellationToken);

    public Task<VenueDetailDto?> GetVenueByIdAsync(int venueId, CancellationToken cancellationToken = default)
        => repository.GetVenueByIdAsync(venueId, cancellationToken);
}
