namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs.Requests;

public record CreateEmployeeRequest(
    string Name,
    string EmployeeNumber,
    string NeoFacePersonId,
    string? Department,
    string? Position,
    string? Email,
    string? Phone,
    bool IsActive,
    DateTime? HireDate,
    Guid? PrimaryLocationId,
    Guid? WorkScheduleId
);
