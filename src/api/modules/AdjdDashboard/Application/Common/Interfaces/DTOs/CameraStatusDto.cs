namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs;

public record CameraStatusDto
{
    public Guid CameraId { get; init; }
    public string CameraName { get; init; } = string.Empty;
    public string CameraType { get; init; } = string.Empty;
    public string LocationName { get; init; } = string.Empty;
    public bool IsOperational { get; init; }
    public int TodayEventCount { get; init; }
    public DateTime? LastEventTime { get; init; }
}
