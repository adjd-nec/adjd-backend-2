using FSH.Starter.AdjdDashboard.Application.Common.DTOs;
using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain.Enums;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly ICameraRepository _cameraRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeEventRepository _eventRepository;
    private readonly IAlertRepository _alertRepository;

    public DashboardService(
        ICameraRepository cameraRepository,
        IEmployeeRepository employeeRepository,
        IEmployeeEventRepository eventRepository,
        IAlertRepository alertRepository)
    {
        _cameraRepository = cameraRepository;
        _employeeRepository = employeeRepository;
        _eventRepository = eventRepository;
        _alertRepository = alertRepository;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        // Get all cameras
        var cameras = await _cameraRepository.GetAllAsync(cancellationToken);
        var activeCameras = cameras.Where(c => c.IsOperational).ToList();

        // Get today's events
        var todayEvents = await _eventRepository.GetTodaysEventsAsync(cancellationToken);
        var todayEntries = todayEvents.Count(e => e.EventType == EventType.Entry);
        var todayExits = todayEvents.Count(e => e.EventType == EventType.Exit);

        // Calculate employees present (simplified logic)
        var employeesPresent = Math.Max(0, todayEntries - todayExits);

        // Get alert counts
        var activeAlerts = await _alertRepository.GetActiveAlertsAsync(cancellationToken);
        var alertCounts = new AlertCountsDto
        {
            MissingEntry = activeAlerts.Count(a => a.AlertType == AlertType.EntryMissing),
            MissingExit = activeAlerts.Count(a => a.AlertType == AlertType.ExitMissing),
            DoubleEntry = activeAlerts.Count(a => a.AlertType == AlertType.DoubleEntry),
            DoubleExit = activeAlerts.Count(a => a.AlertType == AlertType.DoubleExit),
            Away = activeAlerts.Count(a => a.AlertType == AlertType.Away)
        };

        return new DashboardSummaryDto
        {
            TotalEmployeesPresent = employeesPresent,
            TotalCameras = cameras.Count(),
            ActiveCameras = activeCameras.Count,
            InactiveCameras = cameras.Count() - activeCameras.Count,
            TotalActiveAlerts = activeAlerts.Count(),
            TodayEntries = todayEntries,
            TodayExits = todayExits,
            AlertCounts = alertCounts
        };
    }

    public async Task<IEnumerable<EmployeeStatusDto>> GetCurrentEmployeeStatusAsync(CancellationToken cancellationToken = default)
    {
        var employees = await _employeeRepository.GetActiveEmployeesAsync(cancellationToken);
        var employeeStatuses = new List<EmployeeStatusDto>();

        foreach (var employee in employees)
        {
            var lastEvent = await _eventRepository.GetLastEventForEmployeeAsync(employee.Id, cancellationToken);
            var hasEnteredToday = await _eventRepository.HasEmployeeEnteredTodayAsync(employee.Id, cancellationToken);
            var hasExitedToday = await _eventRepository.HasEmployeeExitedTodayAsync(employee.Id, cancellationToken);
            var activeAlertCount = await _alertRepository.GetActiveAlertCountAsync(null, cancellationToken);

            string status;
            if (!hasEnteredToday)
                status = "Not Entered";
            else if (hasEnteredToday && !hasExitedToday)
                status = "Present";
            else if (lastEvent?.EventType == EventType.Entry)
                status = "Present";
            else if (lastEvent?.EventType == EventType.Exit)
                status = "Away";
            else
                status = "Unknown";

            employeeStatuses.Add(new EmployeeStatusDto
            {
                EmployeeId = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                EmployeeName = employee.Name,
                Department = employee.Department ?? "Unknown",
                Status = status,
                LastEventTime = lastEvent?.EventTime,
                LastEventType = lastEvent?.EventType.ToString(),
                LastCameraName = lastEvent?.Camera?.Name,
                ActiveAlertCount = activeAlertCount
            });
        }

        return employeeStatuses.OrderBy(e => e.EmployeeName);
    }

    public async Task<IEnumerable<CameraStatusDto>> GetCameraStatusAsync(CancellationToken cancellationToken = default)
    {
        var cameras = await _cameraRepository.GetAllAsync(cancellationToken);
        var cameraStatuses = new List<CameraStatusDto>();

        foreach (var camera in cameras)
        {
            var todayEvents = await _eventRepository.GetByCameraAsync(camera.Id, DateTime.Today, DateTime.Today.AddDays(1), cancellationToken);
            var lastEvent = todayEvents.OrderByDescending(e => e.EventTime).FirstOrDefault();

            cameraStatuses.Add(new CameraStatusDto
            {
                CameraId = camera.Id,
                CameraName = camera.Name,
                CameraType = camera.CameraType.ToString(),
                LocationName = camera.Location?.Name ?? "Unknown",
                IsOperational = camera.IsOperational,
                TodayEventCount = todayEvents.Count(),
                LastEventTime = lastEvent?.EventTime
            });
        }

        return cameraStatuses.OrderBy(c => c.LocationName).ThenBy(c => c.CameraName);
    }

    public async Task<IEnumerable<AlertSummaryDto>> GetActiveAlertsAsync(CancellationToken cancellationToken = default)
    {
        var alerts = await _alertRepository.GetActiveAlertsAsync(cancellationToken);

        return alerts.Select(alert => new AlertSummaryDto
        {
            AlertId = alert.Id,
            AlertType = alert.AlertType.ToString(),
            Title = alert.Title,
            Description = alert.Description ?? string.Empty,
            Priority = alert.Priority.ToString(),
            GeneratedAt = alert.GeneratedAt,
            EmployeeName = alert.Employee?.Name,
            EmployeeNumber = alert.Employee?.EmployeeNumber,
            CameraName = alert.Camera?.Name,
            LocationName = alert.Camera?.Location?.Name
        });
    }
}
