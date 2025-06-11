using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Enums;

namespace Dashboard.Core.Application.DTOs;
public record EmployeeAlertDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    AlertType AlertType,
    DateTime AlertDateTime,
    string Description,
    bool IsResolved,
    DateTime? ResolvedAt,
    Guid? ResolvedBy,
    Guid LocationId,
    string LocationName,
    Guid? RelatedEventId);
