using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Enums;

namespace Dashboard.Core.Application.DTOs;
public record DashboardCameraDto(
    Guid Id,
    string Name,
    Guid NeoFaceCameraId,
    Guid LocationId,
    string LocationName,
    CameraType CameraType,
    bool IsActive,
    string? IPAddress,
    CameraStatus Status,
    DateTime? LastStatusCheck,
    decimal? PositionX,
    decimal? PositionY,
    Guid? FloorPlanId);
