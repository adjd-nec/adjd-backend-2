using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence;

public class NeoFaceWatchOptions
{
    public string ConnectionString { get; set; } = string.Empty;
}

public class NeoFaceWatchDbContext : DbContext
{
    public NeoFaceWatchDbContext(DbContextOptions<NeoFaceWatchDbContext> options) : base(options)
    {
    }

    // Read-only entities mapping to NeoFace Watch tables
    public DbSet<NfwCamera> Cameras { get; set; }
    public DbSet<NfwWatchList> WatchLists { get; set; }
    public DbSet<NfwMonitoringResult> MonitoringResults { get; set; }
    public DbSet<NfwConfiguration> Configurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to existing NeoFace Watch table structure
        ConfigureNeoFaceWatchMappings(modelBuilder);
    }

    private static void ConfigureNeoFaceWatchMappings(ModelBuilder modelBuilder)
    {
        // Map to existing NeoFace Watch tables
        modelBuilder.Entity<NfwCamera>(entity =>
        {
            entity.ToTable("Cameras", "dbo");
            entity.HasKey(e => e.CameraID);
        });

        modelBuilder.Entity<NfwWatchList>(entity =>
        {
            entity.ToTable("WatchLists", "dbo");
            entity.HasKey(e => e.WatchListID);
        });

        modelBuilder.Entity<NfwMonitoringResult>(entity =>
        {
            entity.ToTable("MonitoringMatchResults", "dbo");
            entity.HasKey(e => e.MatchID);
        });

        modelBuilder.Entity<NfwConfiguration>(entity =>
        {
            entity.ToTable("Configuration", "dbo");
            entity.HasKey(e => e.ConfigKey);
        });
    }
}

// Read-only entities for NeoFace Watch database
public class NfwCamera
{
    public Guid CameraID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public Guid? LocationID { get; set; }
}

public class NfwWatchList
{
    public Guid WatchListID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
}

public class NfwMonitoringResult
{
    public Guid MatchID { get; set; }
    public Guid CameraID { get; set; }
    public Guid PersonID { get; set; }
    public DateTime MatchDateTime { get; set; }
    public decimal MatchScore { get; set; }
}

public class NfwConfiguration
{
    public string ConfigKey { get; set; } = string.Empty;
    public string ConfigValue { get; set; } = string.Empty;
}
