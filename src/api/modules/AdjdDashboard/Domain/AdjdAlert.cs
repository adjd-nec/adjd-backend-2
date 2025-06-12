using FSH.Framework.Core.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdAlert : AuditableEntity<Guid>
{
    [Required]
    public AlertType AlertType { get; set; }

    [Required]
    public AlertStatus Status { get; set; } = AlertStatus.Active;

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public DateTime GeneratedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }
    public DateTime? DismissedAt { get; set; }

    [MaxLength(100)]
    public string? ResolvedBy { get; set; }

    [MaxLength(500)]
    public string? ResolutionNotes { get; set; }

    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    // Employee Relationship
    public Guid? EmployeeId { get; set; }
    public virtual AdjdEmployee? Employee { get; set; }

    // Camera Relationship  
    public Guid? CameraId { get; set; }
    public virtual AdjdCamera? Camera { get; set; }

    // Related Event
    public Guid? RelatedEventId { get; set; }
    public virtual AdjdEmployeeEvent? RelatedEvent { get; set; }
}
