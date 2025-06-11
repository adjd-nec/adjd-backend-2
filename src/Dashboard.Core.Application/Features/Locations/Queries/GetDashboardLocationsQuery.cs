using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dashboard.Core.Application.Contracts;
using Dashboard.Core.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Core.Application.Features.Locations.Queries;
public record GetDashboardLocationsQuery : IRequest<List<DashboardLocationDto>>;

public class GetDashboardLocationsQueryHandler : IRequestHandler<GetDashboardLocationsQuery, List<DashboardLocationDto>>
{
    private readonly IDashboardDbContext _context;
    private readonly IMapper _mapper;

    public GetDashboardLocationsQueryHandler(IDashboardDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DashboardLocationDto>> Handle(GetDashboardLocationsQuery request, CancellationToken cancellationToken)
    {
        var locations = await _context.DashboardLocations
            .Where(l => !l.IsDeleted && l.IsActive)
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<DashboardLocationDto>>(locations);
    }
}
