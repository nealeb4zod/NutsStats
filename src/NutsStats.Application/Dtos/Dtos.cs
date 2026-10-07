namespace NutsStats.Application.Dtos;

public sealed record VenueDto(int Id, string VenueId, string VenueName, DateTime CreatedAt);
public sealed record EventDto(int Id, string EventId, string EventUrl, DateTime? Date, int VenueId);
public sealed record ResultEntryDto(int Id, int? Position, string PlayerName, string PlayerId, int? Points, int EventId);
public sealed record ScrapeSummaryDto(int TotalVenues, int TotalEvents, int TotalResults, DateTime StartedAt, DateTime? FinishedAt, bool Succeeded, string? ErrorMessage);
