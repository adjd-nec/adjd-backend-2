namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs;

public record EmployeeStatusDto
{
    public Guid EmployeeId { get; init; }
    public string EmployeeNumber { get; init; } = string.Empty;
    public string EmployeeName { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty; // Present, Away, Not Entered
    public DateTime? LastEventTime { get; init; }
    public string? LastEventType { get; init; }
    public string? LastCameraName { get; init; }
    public int ActiveAlertCount { get; init; }
}
