using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Application.DTOs;
public record WatchlistConfigurationDto(
    Guid Id,
    Guid LocationId,
    string LocationName,
    Guid NeoFaceWatchlistId,
    string Name,
    bool IsActive,
    decimal MinConfidenceScore,
    bool EnableAlerts);
