using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Enums;

namespace Dashboard.Core.Domain.Entities;
public class EmployeeAlert
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid EmployeeId { get; private set; }
    public string EmployeeName { get; private set; } = default!;
    public AlertType AlertType { get; private set; }
    public DateTime AlertDateTime { get; private set; }
    public string Description { get; private set; } = default!;
    public bool IsResolved { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public Guid? ResolvedBy { get; private set; }
    public Guid LocationId { get; private set; }
    public Guid? RelatedEventId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public DashboardLocation Location { get; private set; } = default!;
    public EmployeeEvent? RelatedEvent { get; private set; }

    private EmployeeAlert() { } // EF Constructor

    public EmployeeAlert(
        Guid employeeId,
        string employeeName,
        AlertType alertType,
        DateTime alertDateTime,
        string description,
        Guid locationId,
        Guid? relatedEventId = null)
    {
        EmployeeId = employeeId;
        EmployeeName = employeeName ?? throw new ArgumentNullException(nameof(employeeName));
        AlertType = alertType;
        AlertDateTime = alertDateTime;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        LocationId = locationId;
        RelatedEventId = relatedEventId;
        IsResolved = false;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework

        // Domain event will be handled later
        // AddDomainEvent(new EmployeeAlertCreatedEvent(this));
    }

    public void Resolve(Guid resolvedBy)
    {
        IsResolved = true;
        ResolvedAt = DateTime.UtcNow;
        ResolvedBy = resolvedBy;
        LastModified = DateTime.UtcNow;
        // Domain event will be handled later
        // AddDomainEvent(new EmployeeAlertResolvedEvent(this));
    }

    public void Delete()
    {
        IsDeleted = true;
        LastModified = DateTime.UtcNow;
    }
}
