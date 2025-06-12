using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdDailyCounter : AuditableEntity<Guid>
{
    [Required]
    public DateTime CounterDate { get; set; }

    // Employee Counts
    public int TotalEmployeesPresent { get; set; } = 0;
    public int TotalEntries { get; set; } = 0;
    public int TotalExits { get; set; } = 0;

    // Alert Counts
    public int MissingEntryAlerts { get; set; } = 0;
    public int MissingExitAlerts { get; set; } = 0;
    public int DoubleCountingAlerts { get; set; } = 0;
    public int AwayAlerts { get; set; } = 0;

    // Camera Status
    public int ActiveCameras { get; set; } = 0;
    public int InactiveCameras { get; set; } = 0;

    // Processing Statistics
    public int EventsProcessed { get; set; } = 0;
    public int EventsSkipped { get; set; } = 0;

    public bool IsReset { get; set; } = false;
    public DateTime? ResetAt { get; set; }

    // Location-specific counters
    public Guid? LocationId { get; set; }
    public virtual AdjdLocation? Location { get; set; }
}
