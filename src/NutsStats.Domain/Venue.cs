namespace NutsStats.Domain;

public class Venue
{
    public int Id { get; set; }
    public string VenueId { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<EventEntry> Events { get; set; } = new();
}

public class EventEntry
{
    public int Id { get; set; }
    public string EventId { get; set; } = string.Empty;
    public string EventUrl { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public int VenueId { get; set; }
    public Venue? Venue { get; set; }
    public List<ResultEntry> Results { get; set; } = new();
}

public class ResultEntry
{
    public int Id { get; set; }
    public int? Position { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string PlayerId { get; set; } = string.Empty;
    public int? Points { get; set; }
    public int EventId { get; set; }
    public EventEntry? Event { get; set; }
}
