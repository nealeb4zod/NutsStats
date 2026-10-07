namespace NutsStats.Application;

public sealed record VenueSummaryDto(
    int Id,
    string VenueId,
    string VenueName,
    int EventCount,
    DateTime CreatedAt);

public sealed record VenueDetailDto(
    int Id,
    string VenueId,
    string VenueName,
    IReadOnlyList<EventSummaryDto> Events,
    DateTime CreatedAt);

public sealed record EventSummaryDto(
    int Id,
    string EventId,
    string EventUrl,
    DateTime? Date,
    int ResultCount);
