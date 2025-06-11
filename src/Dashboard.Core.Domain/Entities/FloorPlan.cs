using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Domain.Entities;
public class FloorPlan
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public Guid LocationId { get; private set; }
    public byte[] ImageData { get; private set; } = default!;
    public string ImageContentType { get; private set; } = default!;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public decimal? Scale { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public DashboardLocation Location { get; private set; } = default!;
    public ICollection<DashboardCamera> Cameras { get; private set; } = new List<DashboardCamera>();

    private FloorPlan() { } // EF Constructor

    public FloorPlan(
        string name,
        Guid locationId,
        byte[] imageData,
        string imageContentType,
        int width,
        int height,
        decimal? scale = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        LocationId = locationId;
        ImageData = imageData ?? throw new ArgumentNullException(nameof(imageData));
        ImageContentType = imageContentType ?? throw new ArgumentNullException(nameof(imageContentType));
        Width = width;
        Height = height;
        Scale = scale;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework
    }

    public void UpdateImage(byte[] imageData, string imageContentType, int width, int height)
    {
        ImageData = imageData ?? throw new ArgumentNullException(nameof(imageData));
        ImageContentType = imageContentType ?? throw new ArgumentNullException(nameof(imageContentType));
        Width = width;
        Height = height;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateScale(decimal scale)
    {
        Scale = scale;
        LastModified = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        LastModified = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        LastModified = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        LastModified = DateTime.UtcNow;
    }
}
