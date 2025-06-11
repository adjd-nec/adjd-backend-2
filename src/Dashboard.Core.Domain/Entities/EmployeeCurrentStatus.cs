using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Domain.Entities;
public class EmployeeCurrentStatus
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public string EmployeeName { get; private set; } = default!;
    public bool IsInside { get; private set; }
    public Guid? LastEntryEvent { get; private set; }
    public Guid? LastExitEvent { get; private set; }
    public DateTime LastEventDateTime { get; private set; }
    public Guid LocationId { get; private set; }
    public DateTime? AwayStartTime { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public DashboardLocation Location { get; private set; } = default!;

    private EmployeeCurrentStatus() { } // EF Constructor

    public EmployeeCurrentStatus(Guid employeeId, string employeeName, Guid locationId)
    {
        EmployeeId = employeeId;
        EmployeeName = employeeName ?? throw new ArgumentNullException(nameof(employeeName));
        LocationId = locationId;
        IsInside = false;
        LastEventDateTime = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework
    }

    public void UpdateStatus(EmployeeEvent employeeEvent)
    {
        LastEventDateTime = employeeEvent.EventDateTime;
        LastModified = DateTime.UtcNow;

        if (employeeEvent.IsEntry())
        {
            IsInside = true;
            LastEntryEvent = employeeEvent.Id;
            AwayStartTime = null;
        }
        else if (employeeEvent.IsExit())
        {
            IsInside = false;
            LastExitEvent = employeeEvent.Id;
            AwayStartTime = employeeEvent.EventDateTime;
        }

        // Domain event will be handled later
        // AddDomainEvent(new EmployeeStatusUpdatedEvent(this, employeeEvent));
    }

    public void Delete()
    {
        IsDeleted = true;
        LastModified = DateTime.UtcNow;
    }
}
