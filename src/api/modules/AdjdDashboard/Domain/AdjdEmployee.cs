using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdEmployee : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required]
    public string NeoFacePersonId { get; set; } = string.Empty; // Maps to NeoFace Watch Person ID

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(100)]
    public string? Position { get; set; }

    [EmailAddress, MaxLength(255)]
    public string? Email { get; set; }

    [Phone, MaxLength(20)]
    public string? Phone { get; set; }

    // Work Configuration
    public bool IsActive { get; set; } = true;
    public DateTime? HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }

    // Location and Schedule
    public Guid? PrimaryLocationId { get; set; }
    public virtual AdjdLocation? PrimaryLocation { get; set; }

    public Guid? WorkScheduleId { get; set; }
    public virtual AdjdWorkSchedule? WorkSchedule { get; set; }

    // Relationships
    public virtual ICollection<AdjdEmployeeEvent> Events { get; set; } = new List<AdjdEmployeeEvent>();
    public virtual ICollection<AdjdAlert> Alerts { get; set; } = new List<AdjdAlert>();
}
