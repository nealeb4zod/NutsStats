using System;
using System.Collections.Generic;

namespace NutsStats.Infrastructure.Data;

public class VenueEntity
{
    public int Id { get; set; }
    public string VenueId { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<EventEntity> Events { get; set; } = new();
}

public class EventEntity
{
    public int Id { get; set; }
    public string EventId { get; set; } = string.Empty;
    public string EventUrl { get; set; } = string.Empty;
    public DateTime? Date { get; set; }

    public int VenueEntityId { get; set; }
    public VenueEntity? Venue { get; set; }

    public List<ResultEntryEntity> Results { get; set; } = new();
}

public class ResultEntryEntity
{
    public int Id { get; set; }
    public int? Position { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string PlayerId { get; set; } = string.Empty;
    public int? Points { get; set; }

    public int EventEntityId { get; set; }
    public EventEntity? Event { get; set; }
}
