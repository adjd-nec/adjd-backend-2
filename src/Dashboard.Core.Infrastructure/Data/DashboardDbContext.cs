using System.Collections.Generic;
using System.Reflection.Emit;
using Dashboard.Core.Application.Contracts;
using Dashboard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Infrastructure.Data;

public class DashboardDbContext : DbContext, IDashboardDbContext
{
    public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options) { }

    public DbSet<EmployeeEvent> EmployeeEvents => Set<EmployeeEvent>();
    public DbSet<EmployeeAlert> EmployeeAlerts => Set<EmployeeAlert>();
    public DbSet<EmployeeCurrentStatus> EmployeeCurrentStatus => Set<EmployeeCurrentStatus>();
    public DbSet<DashboardLocation> DashboardLocations => Set<DashboardLocation>();
    public DbSet<DashboardCamera> DashboardCameras => Set<DashboardCamera>();
    public DbSet<FloorPlan> FloorPlans => Set<FloorPlan>();
    public DbSet<WatchlistConfiguration> WatchlistConfigurations => Set<WatchlistConfiguration>();
    public DbSet<DailyStatistic> DailyStatistics => Set<DailyStatistic>();
    public DbSet<SyncStatus> SyncStatus => Set<SyncStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DashboardDbContext).Assembly);

        // Set default schema for Dashboard module
        modelBuilder.HasDefaultSchema("Dashboard");
    }
}
