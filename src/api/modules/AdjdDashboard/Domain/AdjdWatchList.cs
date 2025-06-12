using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdWatchList : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public string NeoFaceWatchListId { get; set; } = string.Empty; // Maps to NeoFace Watch List ID

    // Working Hours Configuration
    public TimeSpan WorkingHoursStart { get; set; } = new(8, 0, 0);
    public TimeSpan WorkingHoursEnd { get; set; } = new(17, 0, 0);

    // Days of Week (bit flags: Sunday=1, Monday=2, Tuesday=4, etc.)
    public int WorkingDays { get; set; } = 62; // Monday to Friday (2+4+8+16+32)

    // Break Periods (JSON serialized)
    public string? BreakPeriodsJson { get; set; } // [{"Start":"12:00","End":"13:00"}]

    // Alert Thresholds
    public int AwayAlertDurationMinutes { get; set; } = 30;
    public int GracePeriodMinutes { get; set; } = 5;
    public int DoubleCountWindowMinutes { get; set; } = 10;
    public int MissingEventTimeoutMinutes { get; set; } = 60;

    // Behavior Rules
    public bool AllowMultipleEntries { get; set; } = false;
    public bool AllowMultipleExits { get; set; } = false;
    public int BreakTimeFlexibilityMinutes { get; set; } = 15;
    public int ShiftChangeWindowMinutes { get; set; } = 30;

    // Weekend and Holiday Rules
    public bool ApplyRulesOnWeekends { get; set; } = false;
    public bool ApplyRulesOnHolidays { get; set; } = false;

    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // Relationships
    public virtual ICollection<AdjdWorkSchedule> WorkSchedules { get; set; } = new List<AdjdWorkSchedule>();
}
