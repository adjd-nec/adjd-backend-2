using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Application.DTOs;

public record FloorPlanDto(
    Guid Id,
    string Name,
    Guid LocationId,
    string LocationName,
    string ImageContentType,
    int Width,
    int Height,
    decimal? Scale,
    bool IsActive);
