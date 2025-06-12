using FSH.Framework.Core.Domain;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence;

public class AdjdDashboardDbContext : DbContext
{
    public AdjdDashboardDbContext(DbContextOptions<AdjdDashboardDbContext> options) : base(options)
    {
    }

    // Core Entities
    public DbSet<AdjdLocation> Locations { get; set; }
    public DbSet<AdjdCamera> Cameras { get; set; }
    public DbSet<AdjdEmployee> Employees { get; set; }
    public DbSet<AdjdWatchList> WatchLists { get; set; }
    public DbSet<AdjdWorkSchedule> WorkSchedules { get; set; }

    // Event and Alert Entities
    public DbSet<AdjdEmployeeEvent> EmployeeEvents { get; set; }
    public DbSet<AdjdAlert> Alerts { get; set; }
    public DbSet<AdjdDailyCounter> DailyCounters { get; set; }

    // Configuration Entities
    public DbSet<AdjdCameraConfiguration> CameraConfigurations { get; set; }
    public DbSet<AdjdAlertConfiguration> AlertConfigurations { get; set; }
    public DbSet<AdjdSystemConfiguration> SystemConfigurations { get; set; }
    public DbSet<AdjdReportConfiguration> ReportConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Schema
        modelBuilder.HasDefaultSchema("adjd");

        // Indexes for Performance
        ConfigureIndexes(modelBuilder);

        // Relationships
        ConfigureRelationships(modelBuilder);

        // Value Conversions and Constraints
        ConfigureValueConversions(modelBuilder);

        // Seed Data
        SeedInitialData(modelBuilder);
    }

    private static void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // Employee Events - Critical for real-time processing
        modelBuilder.Entity<AdjdEmployeeEvent>()
            .HasIndex(e => new { e.EmployeeId, e.EventTime });

        modelBuilder.Entity<AdjdEmployeeEvent>()
            .HasIndex(e => new { e.CameraId, e.EventTime });

        modelBuilder.Entity<AdjdEmployeeEvent>()
            .HasIndex(e => e.IsProcessed);

        // Alerts - For dashboard performance
        modelBuilder.Entity<AdjdAlert>()
            .HasIndex(e => new { e.Status, e.GeneratedAt });

        modelBuilder.Entity<AdjdAlert>()
            .HasIndex(e => e.AlertType);

        // Daily Counters - For reporting
        modelBuilder.Entity<AdjdDailyCounter>()
            .HasIndex(e => e.CounterDate)
            .IsUnique();

        // Employees - For lookups
        modelBuilder.Entity<AdjdEmployee>()
            .HasIndex(e => e.EmployeeNumber)
            .IsUnique();

        modelBuilder.Entity<AdjdEmployee>()
            .HasIndex(e => e.NeoFacePersonId)
            .IsUnique();

        // Cameras - For lookups
        modelBuilder.Entity<AdjdCamera>()
            .HasIndex(e => e.NeoFaceCameraId)
            .IsUnique();

        // System Configuration - For settings lookup
        modelBuilder.Entity<AdjdSystemConfiguration>()
            .HasIndex(e => e.ConfigKey)
            .IsUnique();
    }

    private static void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // Location -> Cameras (One-to-Many)
        modelBuilder.Entity<AdjdCamera>()
            .HasOne(c => c.Location)
            .WithMany(l => l.Cameras)
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Location -> Employees (One-to-Many)
        modelBuilder.Entity<AdjdEmployee>()
            .HasOne(e => e.PrimaryLocation)
            .WithMany(l => l.Employees)
            .HasForeignKey(e => e.PrimaryLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        // WatchList -> WorkSchedules (One-to-Many)
        modelBuilder.Entity<AdjdWorkSchedule>()
            .HasOne(ws => ws.WatchList)
            .WithMany(wl => wl.WorkSchedules)
            .HasForeignKey(ws => ws.WatchListId)
            .OnDelete(DeleteBehavior.Restrict);

        // WorkSchedule -> Employees (One-to-Many)
        modelBuilder.Entity<AdjdEmployee>()
            .HasOne(e => e.WorkSchedule)
            .WithMany(ws => ws.Employees)
            .HasForeignKey(e => e.WorkScheduleId)
            .OnDelete(DeleteBehavior.SetNull);

        // Camera -> CameraConfiguration (One-to-One)
        modelBuilder.Entity<AdjdCameraConfiguration>()
            .HasOne(cc => cc.Camera)
            .WithOne(c => c.Configuration)
            .HasForeignKey<AdjdCameraConfiguration>(cc => cc.CameraId)
            .OnDelete(DeleteBehavior.Cascade);

        // Employee -> Events (One-to-Many)
        modelBuilder.Entity<AdjdEmployeeEvent>()
            .HasOne(ee => ee.Employee)
            .WithMany(e => e.Events)
            .HasForeignKey(ee => ee.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Camera -> Events (One-to-Many)
        modelBuilder.Entity<AdjdEmployeeEvent>()
            .HasOne(ee => ee.Camera)
            .WithMany(c => c.Events)
            .HasForeignKey(ee => ee.CameraId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee -> Alerts (One-to-Many)
        modelBuilder.Entity<AdjdAlert>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.Alerts)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Camera -> Alerts (One-to-Many)
        modelBuilder.Entity<AdjdAlert>()
            .HasOne(a => a.Camera)
            .WithMany()
            .HasForeignKey(a => a.CameraId)
            .OnDelete(DeleteBehavior.SetNull);

        // Event -> Alert (One-to-One)
        modelBuilder.Entity<AdjdAlert>()
            .HasOne(a => a.RelatedEvent)
            .WithMany()
            .HasForeignKey(a => a.RelatedEventId)
            .OnDelete(DeleteBehavior.SetNull);

        // Location -> DailyCounters (One-to-Many)
        modelBuilder.Entity<AdjdDailyCounter>()
            .HasOne(dc => dc.Location)
            .WithMany()
            .HasForeignKey(dc => dc.LocationId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureValueConversions(ModelBuilder modelBuilder)
    {
        // Enum conversions
        modelBuilder.Entity<AdjdCamera>()
            .Property(e => e.CameraType)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdCamera>()
            .Property(e => e.FlowDirection)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdCamera>()
            .Property(e => e.Priority)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdEmployeeEvent>()
            .Property(e => e.EventType)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdAlert>()
            .Property(e => e.AlertType)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdAlert>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdAlert>()
            .Property(e => e.Priority)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdAlertConfiguration>()
            .Property(e => e.AlertType)
            .HasConversion<int>();

        modelBuilder.Entity<AdjdAlertConfiguration>()
            .Property(e => e.AfterHoursPriority)
            .HasConversion<int>();

        // Decimal precision for coordinates and scores
        modelBuilder.Entity<AdjdCamera>()
            .Property(e => e.FloorPlanX)
            .HasPrecision(18, 6);

        modelBuilder.Entity<AdjdCamera>()
            .Property(e => e.FloorPlanY)
            .HasPrecision(18, 6);

        modelBuilder.Entity<AdjdEmployeeEvent>()
            .Property(e => e.ConfidenceScore)
            .HasPrecision(5, 3);

        modelBuilder.Entity<AdjdCameraConfiguration>()
            .Property(e => e.MinConfidenceThreshold)
            .HasPrecision(5, 3);
    }

    private static void SeedInitialData(ModelBuilder modelBuilder)
    {
        // Default System Configurations
        var systemConfigs = new[]
        {
            new AdjdSystemConfiguration
            {
                //Id = new Guid("11111111-1111-1111-1111-111111111111"),
                ConfigKey = "NeoFaceWatch.PollingIntervalSeconds",
                ConfigValue = "30",
                Description = "How often to poll NeoFace Watch for new events",
                ConfigType = "Integer",
                IsSystem = true,
                Category = "Integration"
            },
            new AdjdSystemConfiguration
            {
                //Id = new Guid("22222222-2222-2222-2222-222222222222"),
                ConfigKey = "Dashboard.RefreshIntervalSeconds",
                ConfigValue = "10",
                Description = "Dashboard real-time refresh interval",
                ConfigType = "Integer",
                IsSystem = true,
                Category = "Dashboard"
            },
            new AdjdSystemConfiguration
            {
                //Id = new Guid("33333333-3333-3333-3333-333333333333"),
                ConfigKey = "Alerts.DefaultAwayDurationMinutes",
                ConfigValue = "30",
                Description = "Default away alert duration in minutes",
                ConfigType = "Integer",
                IsSystem = false,
                Category = "Alerts"
            }
        };

        modelBuilder.Entity<AdjdSystemConfiguration>().HasData(systemConfigs);

        // Default Watch List
        var defaultWatchListId = new Guid("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA");
        modelBuilder.Entity<AdjdWatchList>().HasData(new AdjdWatchList
        {
            //Id = defaultWatchListId,
            Name = "Default Watch List",
            Description = "Default watch list for all employees",
            NeoFaceWatchListId = "default",
            WorkingHoursStart = new TimeSpan(8, 0, 0),
            WorkingHoursEnd = new TimeSpan(17, 0, 0),
            WorkingDays = 62, // Monday to Friday
            AwayAlertDurationMinutes = 30,
            GracePeriodMinutes = 5,
            DoubleCountWindowMinutes = 10,
            MissingEventTimeoutMinutes = 60,
            IsDefault = true,
            IsActive = true
        });

        // Default Work Schedule
        modelBuilder.Entity<AdjdWorkSchedule>().HasData(new AdjdWorkSchedule
        {
            //Id = new Guid("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
            Name = "Standard Business Hours",
            Description = "Standard 8-5 business hours",
            ShiftStart = new TimeSpan(8, 0, 0),
            ShiftEnd = new TimeSpan(17, 0, 0),
            WorkingDays = 62, // Monday to Friday
            CoreHoursRequired = 8,
            IsDefault = true,
            IsActive = true,
            WatchListId = defaultWatchListId
        });

        // Default Alert Configurations
        var alertConfigs = new[]
        {
            new AdjdAlertConfiguration
            {
                //Id = new Guid("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                Name = "Entry Missing Alert",
                AlertType = AlertType.EntryMissing,
                EmailNotificationEnabled = true,
                DashboardNotificationEnabled = true,
                EscalationTimeoutMinutes = 60,
                IsActive = true
            },
            new AdjdAlertConfiguration
            {
                //Id = new Guid("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                Name = "Exit Missing Alert",
                AlertType = AlertType.ExitMissing,
                EmailNotificationEnabled = true,
                DashboardNotificationEnabled = true,
                EscalationTimeoutMinutes = 60,
                AutoResolveEnabled = true,
                AutoResolveAfterMinutes = 480, // 8 hours
                IsActive = true
            }
        };

        modelBuilder.Entity<AdjdAlertConfiguration>().HasData(alertConfigs);
    }
}
