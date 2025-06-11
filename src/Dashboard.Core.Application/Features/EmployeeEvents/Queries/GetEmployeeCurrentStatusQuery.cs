using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.Common;
using Dashboard.Core.Application.Contracts;
using Dashboard.Core.Application.DTOs;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Core.Application.Features.EmployeeEvents.Queries;
public record GetEmployeeCurrentStatusQuery(
    Guid? LocationId = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PaginatedResult<EmployeeCurrentStatusDto>>;

public class GetEmployeeCurrentStatusQueryHandler : IRequestHandler<GetEmployeeCurrentStatusQuery, PaginatedResult<EmployeeCurrentStatusDto>>
{
    private readonly IDashboardDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeeCurrentStatusQueryHandler(IDashboardDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<EmployeeCurrentStatusDto>> Handle(GetEmployeeCurrentStatusQuery request, CancellationToken cancellationToken)
    {
        var query = _context.EmployeeCurrentStatus
            .Include(s => s.Location)
            .Where(s => !s.IsDeleted);

        if (request.LocationId.HasValue)
            query = query.Where(s => s.LocationId == request.LocationId.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var statuses = await query
            .OrderBy(s => s.EmployeeName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var statusDtos = _mapper.Map<List<EmployeeCurrentStatusDto>>(statuses);

        return new PaginatedResult<EmployeeCurrentStatusDto>(statusDtos, totalCount, request.Page, request.PageSize);
    }
}
