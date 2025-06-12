using FSH.Framework.Core.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdAlertConfiguration : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public AlertType AlertType { get; set; }

    // Notification Settings
    public bool EmailNotificationEnabled { get; set; } = true;
    public bool DashboardNotificationEnabled { get; set; } = true;
    public bool SmsNotificationEnabled { get; set; } = false;

    // Escalation Rules
    public int EscalationTimeoutMinutes { get; set; } = 60;
    public string? EscalationEmailList { get; set; } // JSON array

    // Auto-Resolution
    public bool AutoResolveEnabled { get; set; } = false;
    public int AutoResolveAfterMinutes { get; set; } = 240;

    // Business Hours Impact
    public bool DifferentBehaviorAfterHours { get; set; } = false;
    public PriorityLevel AfterHoursPriority { get; set; } = PriorityLevel.Low;

    public bool IsActive { get; set; } = true;
}
