namespace NutsStats.Domain.Models;

public sealed class Venue
{
    public int Id { get; set; }
    public string VenueId { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class EventItem
{
    public int Id { get; set; }
    public string EventId { get; set; } = string.Empty;
    public string EventUrl { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public int VenueId { get; set; }
}

public sealed class ResultEntry
{
    public int Id { get; set; }
    public int? Position { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string PlayerId { get; set; } = string.Empty;
    public int? Points { get; set; }
    public int EventId { get; set; }
}
