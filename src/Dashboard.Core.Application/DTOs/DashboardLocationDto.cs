using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Application.DTOs;
public record DashboardLocationDto(
    Guid Id,
    string Name,
    string? Description,
    string? Address,
    bool IsActive,
    TimeSpan WorkingHoursStart,
    TimeSpan WorkingHoursEnd,
    TimeSpan? BreakTimeStart,
    TimeSpan? BreakTimeEnd,
    int AwayAlertMinutes,
    int? MaxCapacity);
