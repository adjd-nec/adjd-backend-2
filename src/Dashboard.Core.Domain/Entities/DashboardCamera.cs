using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Enums;

namespace Dashboard.Core.Domain.Entities;
public class DashboardCamera
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public Guid NeoFaceCameraId { get; private set; }
    public Guid LocationId { get; private set; }
    public CameraType CameraType { get; private set; }
    public bool IsActive { get; private set; }
    public string? IPAddress { get; private set; }
    public CameraStatus Status { get; private set; }
    public DateTime? LastStatusCheck { get; private set; }
    public decimal? PositionX { get; private set; }
    public decimal? PositionY { get; private set; }
    public Guid? FloorPlanId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public DashboardLocation Location { get; private set; } = default!;
    public FloorPlan? FloorPlan { get; private set; }
    public ICollection<EmployeeEvent> EmployeeEvents { get; private set; } = new List<EmployeeEvent>();

    private DashboardCamera() { } // EF Constructor

    public DashboardCamera(
        string name,
        Guid neoFaceCameraId,
        Guid locationId,
        CameraType cameraType,
        string? ipAddress = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        NeoFaceCameraId = neoFaceCameraId;
        LocationId = locationId;
        CameraType = cameraType;
        IPAddress = ipAddress;
        IsActive = true;
        Status = CameraStatus.Online;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework
    }

    public void UpdateStatus(CameraStatus status)
    {
        var previousStatus = Status;
        Status = status;
        LastStatusCheck = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        // Domain event will be handled later
        // AddDomainEvent(new CameraStatusChangedEvent(this, previousStatus, status));
    }

    public void UpdatePosition(decimal x, decimal y, Guid? floorPlanId = null)
    {
        PositionX = x;
        PositionY = y;
        if (floorPlanId.HasValue)
            FloorPlanId = floorPlanId;
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
