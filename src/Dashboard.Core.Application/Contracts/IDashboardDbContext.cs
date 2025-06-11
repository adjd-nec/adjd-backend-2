using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Dashboard.Core.Application.Contracts;
public interface IDashboardDbContext
{
    DbSet<EmployeeEvent> EmployeeEvents { get; }
    DbSet<EmployeeAlert> EmployeeAlerts { get; }
    DbSet<EmployeeCurrentStatus> EmployeeCurrentStatus { get; }
    DbSet<DashboardLocation> DashboardLocations { get; }
    DbSet<DashboardCamera> DashboardCameras { get; }
    DbSet<FloorPlan> FloorPlans { get; }
    DbSet<WatchlistConfiguration> WatchlistConfigurations { get; }
    DbSet<DailyStatistic> DailyStatistics { get; }
    DbSet<SyncStatus> SyncStatus { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
