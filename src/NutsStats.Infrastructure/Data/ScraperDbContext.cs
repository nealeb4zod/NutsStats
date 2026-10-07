using Microsoft.EntityFrameworkCore;

namespace NutsStats.Infrastructure.Data;

public class ScraperDbContext : DbContext
{
    public ScraperDbContext(DbContextOptions<ScraperDbContext> options) : base(options) { }

    public DbSet<VenueEntity> Venues { get; set; }
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<ResultEntryEntity> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VenueEntity>().HasIndex(v => v.VenueId).IsUnique(false);
        modelBuilder.Entity<EventEntity>().HasIndex(e => new { e.EventId, e.VenueEntityId }).IsUnique();
        modelBuilder.Entity<EventEntity>().HasIndex(e => e.EventUrl);
    }
}
