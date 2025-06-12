namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs.Requests;

public record UpdateLocationRequest(
    string Name,
    string? Description,
    string Building,
    string? Floor,
    string? Zone,
    string? FloorPlanFileName,
    string? FloorPlanContentType,
    byte[]? FloorPlanImage
);
