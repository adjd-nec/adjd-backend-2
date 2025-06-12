namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs;

public record DashboardSummaryDto
{
    public int TotalEmployeesPresent { get; init; }
    public int TotalCameras { get; init; }
    public int ActiveCameras { get; init; }
    public int InactiveCameras { get; init; }
    public int TotalActiveAlerts { get; init; }
    public int TodayEntries { get; init; }
    public int TodayExits { get; init; }
    public AlertCountsDto AlertCounts { get; init; } = new();
    public DateTime LastUpdated { get; init; } = DateTime.Now;
}
