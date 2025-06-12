namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs.Requests;

public record UpdateEmployeeRequest(
    string Name,
    string? Department,
    string? Position,
    string? Email,
    string? Phone,
    bool IsActive,
    DateTime? HireDate,
    DateTime? TerminationDate,
    Guid? PrimaryLocationId,
    Guid? WorkScheduleId
);
