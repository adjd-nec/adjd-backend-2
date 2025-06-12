using FSH.Framework.Core.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdCamera : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public string NeoFaceCameraId { get; set; } = string.Empty; // Maps to NeoFace Watch Camera ID

    // Camera Type Configuration
    [Required]
    public CameraType CameraType { get; set; }

    public FlowDirection FlowDirection { get; set; } = FlowDirection.Bidirectional;

    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    [MaxLength(100)]
    public string? CoverageArea { get; set; }

    // Floor Plan Positioning
    public decimal? FloorPlanX { get; set; }
    public decimal? FloorPlanY { get; set; }

    // Operational Configuration
    public bool IsAwayAlertEligible { get; set; } = true;
    public bool IsOperational { get; set; } = true;
    public TimeSpan? OperationalHoursStart { get; set; }
    public TimeSpan? OperationalHoursEnd { get; set; }

    // Location Relationship
    [Required]
    public Guid LocationId { get; set; }
    public virtual AdjdLocation Location { get; set; } = null!;

    // Configuration Relationship
    public virtual AdjdCameraConfiguration? Configuration { get; set; }

    // Events Relationship
    public virtual ICollection<AdjdEmployeeEvent> Events { get; set; } = new List<AdjdEmployeeEvent>();
}
