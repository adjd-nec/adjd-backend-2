using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Application.DTOs;
public record DailyStatisticDto(
    Guid Id,
    DateOnly Date,
    Guid LocationId,
    string LocationName,
    int TotalEntries,
    int TotalExits,
    int CurrentInside,
    int PeakOccupancy,
    TimeOnly? PeakOccupancyTime,
    int TotalAlerts,
    int ResolvedAlerts);
