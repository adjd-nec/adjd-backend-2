using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdDailyCounter : AuditableEntity<Guid>
{
    [Required]
    public DateTime CounterDate { get; set; }

    public Guid? LocationId { get; set; }
    public virtual AdjdLocation? Location { get; set; }

    // Entry/Exit Counts
    public int TotalEntries { get; set; } = 0;
    public int TotalExits { get; set; } = 0;
    public int CurrentPresent { get; set; } = 0;

    // Alert Counts
    public int MissingEntryAlerts { get; set; } = 0;
    public int MissingExitAlerts { get; set; } = 0;
    public int DoubleEntryAlerts { get; set; } = 0;
    public int DoubleExitAlerts { get; set; } = 0;
    public int AwayAlerts { get; set; } = 0;

    // Peak Statistics
    public int PeakPresent { get; set; } = 0;
    public DateTime? PeakTime { get; set; }

    // Reset Information
    public DateTime LastResetAt { get; set; }
    public bool IsResetComplete { get; set; } = true;
}
