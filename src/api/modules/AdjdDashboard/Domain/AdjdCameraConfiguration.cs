using FSH.Framework.Core.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdCameraConfiguration : AuditableEntity<Guid>
{
    [Required]
    public Guid CameraId { get; set; }
    public virtual AdjdCamera Camera { get; set; } = null!;

    // Detection Thresholds
    public decimal MinConfidenceThreshold { get; set; } = 0.8m;
    public decimal MinFaceQualityScore { get; set; } = 0.7m;
    public decimal MinFrontalFaceScore { get; set; } = 0.6m;

    // Processing Settings
    public bool EnableRealTimeProcessing { get; set; } = true;
    public bool EnableImageCapture { get; set; } = true;
    public bool EnableEventLogging { get; set; } = true;

    // Alert Configuration
    public bool EnableMissingEventAlerts { get; set; } = true;
    public bool EnableDoubleCountingAlerts { get; set; } = true;
    public bool EnableAwayAlerts { get; set; } = true;

    // Timing Configuration
    public int EventProcessingDelayMs { get; set; } = 1000;
    public int MaxEventsPerMinute { get; set; } = 60;

    // Maintenance
    public DateTime? LastCalibrationDate { get; set; }
    public string? CalibrationNotes { get; set; }
}
