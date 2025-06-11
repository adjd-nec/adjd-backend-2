using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Enums;

namespace Dashboard.Core.Application.DTOs;
public record EmployeeEventDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    EventType EventType,
    Guid CameraId,
    string CameraName,
    Guid LocationId,
    string LocationName,
    DateTime EventDateTime,
    decimal? ConfidenceScore,
    bool IsProcessed,
    Guid? NeoFaceMatchId);
