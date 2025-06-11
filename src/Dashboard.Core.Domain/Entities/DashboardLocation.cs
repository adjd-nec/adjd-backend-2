using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Domain.Entities;
public class DashboardLocation
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public TimeSpan WorkingHoursStart { get; private set; }
    public TimeSpan WorkingHoursEnd { get; private set; }
    public TimeSpan? BreakTimeStart { get; private set; }
    public TimeSpan? BreakTimeEnd { get; private set; }
    public int AwayAlertMinutes { get; private set; }
    public int? MaxCapacity { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public ICollection<DashboardCamera> Cameras { get; private set; } = new List<DashboardCamera>();
    public ICollection<EmployeeEvent> EmployeeEvents { get; private set; } = new List<EmployeeEvent>();
    public ICollection<EmployeeAlert> EmployeeAlerts { get; private set; } = new List<EmployeeAlert>();
    public ICollection<EmployeeCurrentStatus> EmployeeStatuses { get; private set; } = new List<EmployeeCurrentStatus>();
    public ICollection<FloorPlan> FloorPlans { get; private set; } = new List<FloorPlan>();
    public ICollection<WatchlistConfiguration> WatchlistConfigurations { get; private set; } = new List<WatchlistConfiguration>();
    public ICollection<DailyStatistic> DailyStatistics { get; private set; } = new List<DailyStatistic>();

    private DashboardLocation() { } // EF Constructor

    public DashboardLocation(
        string name,
        string? description = null,
        string? address = null,
        TimeSpan? workingHoursStart = null,
        TimeSpan? workingHoursEnd = null,
        int awayAlertMinutes = 60)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        Address = address;
        IsActive = true;
        WorkingHoursStart = workingHoursStart ?? new TimeSpan(8, 0, 0);
        WorkingHoursEnd = workingHoursEnd ?? new TimeSpan(17, 0, 0);
        AwayAlertMinutes = awayAlertMinutes;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework
    }

    public void UpdateWorkingHours(TimeSpan start, TimeSpan end)
    {
        WorkingHoursStart = start;
        WorkingHoursEnd = end;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateBreakTime(TimeSpan? start, TimeSpan? end)
    {
        BreakTimeStart = start;
        BreakTimeEnd = end;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateAwayAlertMinutes(int minutes)
    {
        if (minutes <= 0)
            throw new ArgumentException("Away alert minutes must be greater than 0", nameof(minutes));

        AwayAlertMinutes = minutes;
        LastModified = DateTime.UtcNow;
    }

    public bool IsWorkingHours(DateTime dateTime)
    {
        var time = dateTime.TimeOfDay;
        return time >= WorkingHoursStart && time <= WorkingHoursEnd;
    }

    public bool IsBreakTime(DateTime dateTime)
    {
        if (!BreakTimeStart.HasValue || !BreakTimeEnd.HasValue)
            return false;

        var time = dateTime.TimeOfDay;
        return time >= BreakTimeStart.Value && time <= BreakTimeEnd.Value;
    }

    public void Delete()
    {
        IsDeleted = true;
        LastModified = DateTime.UtcNow;
    }
}
