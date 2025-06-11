using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Core.Application.Features.Dashboard.Queries;
public record DashboardStatisticsDto(
    int TotalLocations,
    int ActiveCameras,
    int OfflineCameras,
    int TotalEmployeesInside,
    int ActiveAlerts,
    int TodayEntries,
    int TodayExits,
    DateTime LastUpdated);

public record GetDashboardStatisticsQuery(Guid? LocationId = null) : IRequest<DashboardStatisticsDto>;

public class GetDashboardStatisticsQueryHandler : IRequestHandler<GetDashboardStatisticsQuery, DashboardStatisticsDto>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardStatisticsQueryHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatisticsDto> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        // Base queries
        var locationQuery = _context.DashboardLocations.Where(l => !l.IsDeleted && l.IsActive);
        var cameraQuery = _context.DashboardCameras.Where(c => !c.IsDeleted);
        var statusQuery = _context.EmployeeCurrentStatus.Where(s => !s.IsDeleted);
        var alertQuery = _context.EmployeeAlerts.Where(a => !a.IsDeleted && !a.IsResolved);
        var eventQuery = _context.EmployeeEvents.Where(e => !e.IsDeleted && e.EventDateTime >= today);

        // Apply location filter if specified
        if (request.LocationId.HasValue)
        {
            locationQuery = locationQuery.Where(l => l.Id == request.LocationId.Value);
            cameraQuery = cameraQuery.Where(c => c.LocationId == request.LocationId.Value);
            statusQuery = statusQuery.Where(s => s.LocationId == request.LocationId.Value);
            alertQuery = alertQuery.Where(a => a.LocationId == request.LocationId.Value);
            eventQuery = eventQuery.Where(e => e.LocationId == request.LocationId.Value);
        }

        // Execute queries
        var totalLocations = await locationQuery.CountAsync(cancellationToken);
        var activeCameras = await cameraQuery.CountAsync(c => c.IsActive && c.Status == Domain.Enums.CameraStatus.Online, cancellationToken);
        var offlineCameras = await cameraQuery.CountAsync(c => c.IsActive && c.Status == Domain.Enums.CameraStatus.Offline, cancellationToken);
        var totalEmployeesInside = await statusQuery.CountAsync(s => s.IsInside, cancellationToken);
        var activeAlerts = await alertQuery.CountAsync(cancellationToken);
        var todayEntries = await eventQuery.CountAsync(e => e.EventType == Domain.Enums.EventType.Entry, cancellationToken);
        var todayExits = await eventQuery.CountAsync(e => e.EventType == Domain.Enums.EventType.Exit, cancellationToken);

        return new DashboardStatisticsDto(
            totalLocations,
            activeCameras,
            offlineCameras,
            totalEmployeesInside,
            activeAlerts,
            todayEntries,
            todayExits,
            DateTime.UtcNow);
    }
}
