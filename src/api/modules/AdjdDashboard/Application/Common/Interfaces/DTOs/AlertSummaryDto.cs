namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs;

public record AlertSummaryDto
{
    public Guid AlertId { get; init; }
    public string AlertType { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public DateTime GeneratedAt { get; init; }
    public string? EmployeeName { get; init; }
    public string? EmployeeNumber { get; init; }
    public string? CameraName { get; init; }
    public string? LocationName { get; init; }
}
