using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Application.DTOs;
public record EmployeeCurrentStatusDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    bool IsInside,
    DateTime LastEventDateTime,
    Guid LocationId,
    string LocationName,
    DateTime? AwayStartTime,
    int? AwayMinutes);
