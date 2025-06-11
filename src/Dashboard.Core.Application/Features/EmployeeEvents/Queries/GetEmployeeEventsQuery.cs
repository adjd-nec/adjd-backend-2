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
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Core.Application.Features.EmployeeEvents.Queries
{
    public record GetEmployeeEventsQuery(
    Guid? EmployeeId = null,
    Guid? LocationId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    EventType? EventType = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PaginatedResult<EmployeeEventDto>>;

    public class GetEmployeeEventsQueryHandler : IRequestHandler<GetEmployeeEventsQuery, PaginatedResult<EmployeeEventDto>>
    {
        private readonly IDashboardDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeEventsQueryHandler(IDashboardDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<EmployeeEventDto>> Handle(GetEmployeeEventsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.EmployeeEvents
                .Include(e => e.Camera)
                .Include(e => e.Location)
                .Where(e => !e.IsDeleted);

            if (request.EmployeeId.HasValue)
                query = query.Where(e => e.EmployeeId == request.EmployeeId.Value);

            if (request.LocationId.HasValue)
                query = query.Where(e => e.LocationId == request.LocationId.Value);

            if (request.StartDate.HasValue)
                query = query.Where(e => e.EventDateTime >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(e => e.EventDateTime <= request.EndDate.Value);

            if (request.EventType.HasValue)
                query = query.Where(e => e.EventType == request.EventType.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var events = await query
                .OrderByDescending(e => e.EventDateTime)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var eventDtos = _mapper.Map<List<EmployeeEventDto>>(events);

            return new PaginatedResult<EmployeeEventDto>(eventDtos, totalCount, request.Page, request.PageSize);
        }
    }
}
