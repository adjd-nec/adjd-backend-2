using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdCameraConfiguration : AuditableEntity<Guid>
{
    [Required]
    public Guid CameraId { get; set; }
    public virtual AdjdCamera Camera { get; set; } = null!;

    // Detection Settings
    public decimal MinConfidenceThreshold { get; set; } = 0.7m;
    public int EventCooldownSeconds { get; set; } = 30;
    public bool EnableDuplicateDetection { get; set; } = true;

    // Alert Settings
    public bool GenerateAlerts { get; set; } = true;
    public bool NotifyOnHighPriority { get; set; } = true;

    // Processing Settings
    public bool ProcessEvents { get; set; } = true;
    public int MaxEventsPerMinute { get; set; } = 60;

    // Maintenance
    public DateTime? LastCalibrationDate { get; set; }
    public string? CalibrationNotes { get; set; }
}
