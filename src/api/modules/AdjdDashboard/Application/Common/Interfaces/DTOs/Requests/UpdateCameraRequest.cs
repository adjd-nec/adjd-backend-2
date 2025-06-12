using FSH.Starter.AdjdDashboard.Domain.Enums;

namespace FSH.Starter.AdjdDashboard.Application.Common.DTOs.Requests;

public record UpdateCameraRequest(
    string Name,
    string? Description,
    CameraType CameraType,
    FlowDirection FlowDirection,
    PriorityLevel Priority,
    string? CoverageArea,
    decimal? FloorPlanX,
    decimal? FloorPlanY,
    bool IsAwayAlertEligible,
    bool IsOperational,
    TimeSpan? OperationalHoursStart,
    TimeSpan? OperationalHoursEnd,
    Guid LocationId
);
