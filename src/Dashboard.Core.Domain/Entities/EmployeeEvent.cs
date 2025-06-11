using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Enums;

namespace Dashboard.Core.Domain.Entities
{
    public class EmployeeEvent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid EmployeeId { get; private set; }
        public string EmployeeName { get; private set; } = default!;
        public EventType EventType { get; private set; }
        public Guid CameraId { get; private set; }
        public Guid LocationId { get; private set; }
        public DateTime EventDateTime { get; private set; }
        public decimal? ConfidenceScore { get; private set; }
        public bool IsProcessed { get; private set; }
        public Guid? NeoFaceMatchId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime? LastModified { get; private set; }
        public Guid? LastModifiedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        // Navigation Properties
        public DashboardCamera Camera { get; private set; } = default!;
        public DashboardLocation Location { get; private set; } = default!;

        private EmployeeEvent() { } // EF Constructor

        public EmployeeEvent(
            Guid employeeId,
            string employeeName,
            EventType eventType,
            Guid cameraId,
            Guid locationId,
            DateTime eventDateTime,
            decimal? confidenceScore = null,
            Guid? neoFaceMatchId = null)
        {
            EmployeeId = employeeId;
            EmployeeName = employeeName ?? throw new ArgumentNullException(nameof(employeeName));
            EventType = eventType;
            CameraId = cameraId;
            LocationId = locationId;
            EventDateTime = eventDateTime;
            ConfidenceScore = confidenceScore;
            NeoFaceMatchId = neoFaceMatchId;
            IsProcessed = false;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = Guid.Empty; // Will be set by framework

            // Domain event will be handled later
            // AddDomainEvent(new EmployeeEventCreatedEvent(this));
        }

        public void MarkAsProcessed()
        {
            IsProcessed = true;
            LastModified = DateTime.UtcNow;
            // Domain event will be handled later
            // AddDomainEvent(new EmployeeEventProcessedEvent(this));
        }

        public bool IsEntry() => EventType == EventType.Entry;
        public bool IsExit() => EventType == EventType.Exit;

        public void Delete()
        {
            IsDeleted = true;
            LastModified = DateTime.UtcNow;
        }
    }
}
