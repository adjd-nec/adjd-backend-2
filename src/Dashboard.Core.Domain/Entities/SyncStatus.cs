using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Domain.Entities;
public class SyncStatus
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string EntityType { get; private set; } = default!;
    public DateTime LastSyncDateTime { get; private set; }
    public Guid? LastSyncId { get; private set; }
    public bool IsSuccessful { get; private set; }
    public string? ErrorMessage { get; private set; }
    public int RecordsProcessed { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    private SyncStatus() { } // EF Constructor

    public SyncStatus(
        string entityType,
        DateTime lastSyncDateTime,
        bool isSuccessful,
        int recordsProcessed,
        string? errorMessage = null)
    {
        EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        LastSyncDateTime = lastSyncDateTime;
        IsSuccessful = isSuccessful;
        RecordsProcessed = recordsProcessed;
        ErrorMessage = errorMessage;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework
    }

    public void UpdateSync(DateTime syncDateTime, bool isSuccessful, int recordsProcessed, string? errorMessage = null)
    {
        LastSyncDateTime = syncDateTime;
        IsSuccessful = isSuccessful;
        RecordsProcessed = recordsProcessed;
        ErrorMessage = errorMessage;
        LastModified = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        LastModified = DateTime.UtcNow;
    }
}
