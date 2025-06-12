using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdReportConfiguration : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string ReportName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    // Report Settings
    public string ReportType { get; set; } = "Attendance"; // Attendance, Alerts, Camera, Custom
    public string? FilterCriteria { get; set; } // JSON serialized filters

    // Scheduling
    public bool IsScheduled { get; set; } = false;
    public string? ScheduleCron { get; set; } // Cron expression
    public string? EmailRecipients { get; set; } // JSON array

    // Export Settings
    public string? DefaultExportFormat { get; set; } = "PDF"; // PDF, Excel, CSV
    public bool IncludeCharts { get; set; } = true;
    public bool IncludeImages { get; set; } = false;

    // Retention
    public int RetainReportsForDays { get; set; } = 90;

    public bool IsActive { get; set; } = true;
}
