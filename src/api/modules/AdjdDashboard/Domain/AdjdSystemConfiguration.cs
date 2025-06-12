using FSH.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.AdjdDashboard.Domain;

public class AdjdSystemConfiguration : AuditableEntity<Guid>
{
    [Required, MaxLength(100)]
    public string ConfigKey { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string ConfigValue { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string ConfigType { get; set; } = "String"; // String, Integer, Boolean, JSON

    public bool IsSystem { get; set; } = false; // System configs cannot be deleted
    public bool IsEncrypted { get; set; } = false;

    [MaxLength(100)]
    public string? Category { get; set; }
}
