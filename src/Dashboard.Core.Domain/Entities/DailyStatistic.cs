using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Domain.Entities
{
public class DailyStatistic
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateOnly Date { get; private set; }
    public Guid LocationId { get; private set; }
    public int TotalEntries { get; private set; }
    public int TotalExits { get; private set; }
    public int CurrentInside { get; private set; }
    public int PeakOccupancy { get; private set; }
    public TimeOnly? PeakOccupancyTime { get; private set; }
    public int TotalAlerts { get; private set; }
    public int ResolvedAlerts { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public DashboardLocation Location { get; private set; } = default!;

    private DailyStatistic() { } // EF Constructor

    public DailyStatistic(
        DateOnly date,
        Guid locationId,
        int totalEntries,
        int totalExits,
        int currentInside,
        int peakOccupancy,
        TimeOnly? peakOccupancyTime,
        int totalAlerts,
        int resolvedAlerts)
    {
        Date = date;
        LocationId = locationId;
        TotalEntries = totalEntries;
        TotalExits = totalExits;
        CurrentInside = currentInside;
        PeakOccupancy = peakOccupancy;
        PeakOccupancyTime = peakOccupancyTime;
        TotalAlerts = totalAlerts;
        ResolvedAlerts = resolvedAlerts;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework
    }

    public void UpdateStatistics(
        int totalEntries,
        int totalExits,
        int currentInside,
        int peakOccupancy,
        TimeOnly? peakOccupancyTime,
        int totalAlerts,
        int resolvedAlerts)
    {
        TotalEntries = totalEntries;
        TotalExits = totalExits;
        CurrentInside = currentInside;
        PeakOccupancy = peakOccupancy;
        PeakOccupancyTime = peakOccupancyTime;
        TotalAlerts = totalAlerts;
        ResolvedAlerts = resolvedAlerts;
        LastModified = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        LastModified = DateTime.UtcNow;
    }
}
}
