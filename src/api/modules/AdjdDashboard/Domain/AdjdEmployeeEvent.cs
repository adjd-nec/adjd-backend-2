using FSH.Framework.Core.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdEmployeeEvent : AuditableEntity<Guid>
{
    [Required]
    public Guid EmployeeId { get; set; }
    public virtual AdjdEmployee Employee { get; set; } = null!;

    [Required]
    public Guid CameraId { get; set; }
    public virtual AdjdCamera Camera { get; set; } = null!;

    [Required]
    public DateTime EventTime { get; set; }

    [Required]
    public EventType EventType { get; set; }

    // NeoFace Watch Integration
    public string? NeoFaceEventId { get; set; }
    public decimal? ConfidenceScore { get; set; }

    // Processing Information
    public bool IsProcessed { get; set; } = false;
    public DateTime? ProcessedAt { get; set; }
    public bool IsValidated { get; set; } = true;

    // Additional Context
    public string? Notes { get; set; }
    public byte[]? EventImage { get; set; }
}
