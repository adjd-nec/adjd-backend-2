using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.Common;
using Dashboard.Core.Application.Contracts;
using Dashboard.Core.Application.DTOs;
using Dashboard.Core.Domain.Enums;
using MapsterMapper;
using MediatR;

namespace Dashboard.Core.Application.Features.Alerts.Queries;
public record GetActiveAlertsQuery(
    Guid? LocationId = null,
    AlertType? AlertType = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PaginatedResult<EmployeeAlertDto>>;

public class GetActiveAlertsQueryHandler : IRequestHandler<GetActiveAlertsQuery, PaginatedResult<EmployeeAlertDto>>
{
    private readonly IDashboardDbContext _context;
    private readonly IMapper _mapper;

    public GetActiveAlertsQueryHandler(IDashboardDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<EmployeeAlertDto>> Handle(GetActiveAlertsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.EmployeeAlerts
            .Include(a => a.Location)
            .Include(a => a.RelatedEvent)
            .Where(a => !a.IsDeleted && !a.IsResolved);

        if (request.LocationId.HasValue)
            query = query.Where(a => a.LocationId == request.LocationId.Value);

        if (request.AlertType.HasValue)
            query = query.Where(a => a.AlertType == request.AlertType.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var alerts = await query
            .OrderByDescending(a => a.AlertDateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var alertDtos = _mapper.Map<List<EmployeeAlertDto>>(alerts);

        return new PaginatedResult<EmployeeAlertDto>(alertDtos, totalCount, request.Page, request.PageSize);
    }
}
