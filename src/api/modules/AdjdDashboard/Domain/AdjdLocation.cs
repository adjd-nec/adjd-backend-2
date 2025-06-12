using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdLocation : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required, MaxLength(100)]
    public string Building { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Floor { get; set; }

    [MaxLength(100)]
    public string? Zone { get; set; }

    // Floor Plan Configuration
    public byte[]? FloorPlanImage { get; set; }
    public string? FloorPlanFileName { get; set; }
    public string? FloorPlanContentType { get; set; }

    // Relationships
    public virtual ICollection<AdjdCamera> Cameras { get; set; } = new List<AdjdCamera>();
    public virtual ICollection<AdjdEmployee> Employees { get; set; } = new List<AdjdEmployee>();
}
