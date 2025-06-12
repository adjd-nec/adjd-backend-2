using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdWorkSchedule : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Schedule Details
    public TimeSpan? ShiftStart { get; set; }
    public TimeSpan? ShiftEnd { get; set; }
    public int WorkingDays { get; set; } = 62; // Monday to Friday

    // Flexible Work Arrangements
    public bool IsFlexibleHours { get; set; } = false;
    public TimeSpan? FlexibleStartEarliest { get; set; }
    public TimeSpan? FlexibleStartLatest { get; set; }
    public int CoreHoursRequired { get; set; } = 8;

    // Special Arrangements
    public bool AllowWorkFromHome { get; set; } = false;
    public string? WorkFromHomeDays { get; set; } // JSON: ["Monday", "Friday"]

    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // Watch List Relationship
    [Required]
    public Guid WatchListId { get; set; }
    public virtual AdjdWatchList WatchList { get; set; } = null!;

    // Employees Relationship
    public virtual ICollection<AdjdEmployee> Employees { get; set; } = new List<AdjdEmployee>();
}
