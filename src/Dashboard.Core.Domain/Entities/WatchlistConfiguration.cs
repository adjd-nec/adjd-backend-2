using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Domain.Entities;
public class WatchlistConfiguration
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid LocationId { get; private set; }
    public Guid NeoFaceWatchlistId { get; private set; }
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public decimal MinConfidenceScore { get; private set; }
    public bool EnableAlerts { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime? LastModified { get; private set; }
    public Guid? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation Properties
    public DashboardLocation Location { get; private set; } = default!;

    private WatchlistConfiguration() { } // EF Constructor

    public WatchlistConfiguration(
        Guid locationId,
        Guid neoFaceWatchlistId,
        string name,
        decimal minConfidenceScore = 80.00m)
    {
        LocationId = locationId;
        NeoFaceWatchlistId = neoFaceWatchlistId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsActive = true;
        MinConfidenceScore = minConfidenceScore;
        EnableAlerts = true;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = Guid.Empty; // Will be set by framework
    }

    public void UpdateConfidenceScore(decimal minConfidenceScore)
    {
        if (minConfidenceScore < 0 || minConfidenceScore > 100)
            throw new ArgumentException("Confidence score must be between 0 and 100");

        MinConfidenceScore = minConfidenceScore;
        LastModified = DateTime.UtcNow;
    }

    public void EnableAlerting()
    {
        EnableAlerts = true;
        LastModified = DateTime.UtcNow;
    }

    public void DisableAlerting()
    {
        EnableAlerts = false;
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
