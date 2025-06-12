using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Services;

public interface IAlertGenerationService
{
    Task ProcessEventForAlertsAsync(AdjdEmployeeEvent employeeEvent, CancellationToken cancellationToken = default);
    Task GenerateMissingExitAlertsAsync(CancellationToken cancellationToken = default);
    Task GenerateAwayAlertsAsync(CancellationToken cancellationToken = default);
    Task ResolveAlertsAsync(Guid employeeId, AlertType alertType, CancellationToken cancellationToken = default);
}

public class AlertGenerationService : IAlertGenerationService
{
    private readonly AdjdDashboardDbContext _context;
    private readonly IEmployeeEventRepository _eventRepository;
    private readonly IWatchListRepository _watchListRepository;
    private readonly ILogger<AlertGenerationService> _logger;

    public AlertGenerationService(
        AdjdDashboardDbContext context,
        IEmployeeEventRepository eventRepository,
        IWatchListRepository watchListRepository,
        ILogger<AlertGenerationService> logger)
    {
        _context = context;
        _eventRepository = eventRepository;
        _watchListRepository = watchListRepository;
        _logger = logger;
    }

    public async Task ProcessEventForAlertsAsync(AdjdEmployeeEvent employeeEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            await GenerateEntryMissingAlertAsync(employeeEvent, cancellationToken);
            await GenerateDoubleCountingAlertAsync(employeeEvent, cancellationToken);
            await ResolveExistingAlertsAsync(employeeEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing event for alerts: {EventId}", employeeEvent.Id);
            throw;
        }
    }

    private async Task GenerateEntryMissingAlertAsync(AdjdEmployeeEvent employeeEvent, CancellationToken cancellationToken)
    {
        // Entry Missing: Exit detected without corresponding entry
        if (employeeEvent.EventType != EventType.Exit)
            return;

        var today = employeeEvent.EventTime.Date;
        var hasEntryToday = await _eventRepository.HasEmployeeEnteredTodayAsync(employeeEvent.EmployeeId, cancellationToken);

        if (!hasEntryToday)
        {
            // Check if alert already exists for today
            var existingAlert = await _context.Alerts
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeEvent.EmployeeId &&
                                         a.AlertType == AlertType.EntryMissing &&
                                         a.Status == AlertStatus.Active &&
                                         a.GeneratedAt.Date == today, cancellationToken);

            if (existingAlert == null)
            {
                var alert = new AdjdAlert
                {
                    //Id = Guid.NewGuid(),
                    AlertType = AlertType.EntryMissing,
                    Status = AlertStatus.Active,
                    Title = "Entry Missing",
                    Description = $"Employee {employeeEvent.Employee?.Name} ({employeeEvent.Employee?.EmployeeNumber}) exited without entry record",
                    GeneratedAt = employeeEvent.EventTime,
                    Priority = PriorityLevel.Medium,
                    EmployeeId = employeeEvent.EmployeeId,
                    CameraId = employeeEvent.CameraId,
                    RelatedEventId = employeeEvent.Id
                };

                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Generated Entry Missing alert for employee {EmployeeId}", employeeEvent.EmployeeId);
            }
        }
    }

    private async Task GenerateDoubleCountingAlertAsync(AdjdEmployeeEvent employeeEvent, CancellationToken cancellationToken)
    {
        var today = employeeEvent.EventTime.Date;
        var tomorrow = today.AddDays(1);

        // Get all events for this employee today
        var todayEvents = await _context.EmployeeEvents
            .Where(e => e.EmployeeId == employeeEvent.EmployeeId &&
                       e.EventTime >= today &&
                       e.EventTime < tomorrow &&
                       e.Id != employeeEvent.Id)
            .OrderBy(e => e.EventTime)
            .ToListAsync(cancellationToken);

        // Check for double entry
        if (employeeEvent.EventType == EventType.Entry)
        {
            var lastEvent = todayEvents.LastOrDefault();
            if (lastEvent?.EventType == EventType.Entry)
            {
                await CreateDoubleCountingAlert(employeeEvent, AlertType.DoubleEntry, "Double Entry", cancellationToken);
            }
        }

        // Check for double exit
        if (employeeEvent.EventType == EventType.Exit)
        {
            var lastEvent = todayEvents.LastOrDefault();
            if (lastEvent?.EventType == EventType.Exit)
            {
                await CreateDoubleCountingAlert(employeeEvent, AlertType.DoubleExit, "Double Exit", cancellationToken);
            }
        }
    }

    private async Task CreateDoubleCountingAlert(AdjdEmployeeEvent employeeEvent, AlertType alertType, string title, CancellationToken cancellationToken)
    {
        // Check if alert already exists for today
        var today = employeeEvent.EventTime.Date;
        var existingAlert = await _context.Alerts
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeEvent.EmployeeId &&
                                     a.AlertType == alertType &&
                                     a.Status == AlertStatus.Active &&
                                     a.GeneratedAt.Date == today, cancellationToken);

        if (existingAlert == null)
        {
            var alert = new AdjdAlert
            {
                //Id = Guid.NewGuid(),
                AlertType = alertType,
                Status = AlertStatus.Active,
                Title = title,
                Description = $"Employee {employeeEvent.Employee?.Name} ({employeeEvent.Employee?.EmployeeNumber}) has multiple {employeeEvent.EventType.ToString().ToLower()} events",
                GeneratedAt = employeeEvent.EventTime,
                Priority = PriorityLevel.Low,
                EmployeeId = employeeEvent.EmployeeId,
                CameraId = employeeEvent.CameraId,
                RelatedEventId = employeeEvent.Id
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Generated {AlertType} alert for employee {EmployeeId}", alertType, employeeEvent.EmployeeId);
        }
    }

    private async Task ResolveExistingAlertsAsync(AdjdEmployeeEvent employeeEvent, CancellationToken cancellationToken)
    {
        // Resolve Entry Missing alerts if employee enters
        if (employeeEvent.EventType == EventType.Entry)
        {
            await ResolveAlertsAsync(employeeEvent.EmployeeId, AlertType.EntryMissing, cancellationToken);
        }

        // Resolve Away alerts if employee returns
        if (employeeEvent.EventType == EventType.Entry)
        {
            await ResolveAlertsAsync(employeeEvent.EmployeeId, AlertType.Away, cancellationToken);
        }
    }

    public async Task GenerateMissingExitAlertsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating missing exit alerts for employees who haven't exited");

        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);

        // Find employees who entered yesterday but didn't exit
        var employeesWithMissingExit = await _context.EmployeeEvents
            .Where(e => e.EventTime >= yesterday &&
                       e.EventTime < today &&
                       e.EventType == EventType.Entry)
            .GroupBy(e => e.EmployeeId)
            .Where(g => !_context.EmployeeEvents
                .Any(exit => exit.EmployeeId == g.Key &&
                            exit.EventTime >= yesterday &&
                            exit.EventTime < today &&
                            exit.EventType == EventType.Exit))
            .Select(g => g.Key)
            .ToListAsync(cancellationToken);

        foreach (var employeeId in employeesWithMissingExit)
        {
            // Check if alert already exists
            var existingAlert = await _context.Alerts
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId &&
                                         a.AlertType == AlertType.ExitMissing &&
                                         a.Status == AlertStatus.Active &&
                                         a.GeneratedAt.Date == yesterday, cancellationToken);

            if (existingAlert == null)
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                var alert = new AdjdAlert
                {
                    //Id = Guid.NewGuid(),
                    AlertType = AlertType.ExitMissing,
                    Status = AlertStatus.Active,
                    Title = "Exit Missing",
                    Description = $"Employee {employee?.Name} ({employee?.EmployeeNumber}) entered but didn't exit",
                    GeneratedAt = DateTime.UtcNow,
                    Priority = PriorityLevel.Medium,
                    EmployeeId = employeeId
                };

                _context.Alerts.Add(alert);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Generated missing exit alerts for {Count} employees", employeesWithMissingExit.Count);
    }

    public async Task GenerateAwayAlertsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking for employees who are away too long");

        var defaultWatchList = await _watchListRepository.GetDefaultWatchListAsync(cancellationToken);
        if (defaultWatchList == null)
        {
            _logger.LogWarning("No default watch list found for away alert processing");
            return;
        }

        var awayThresholdMinutes = defaultWatchList.AwayAlertDurationMinutes;
        var thresholdTime = DateTime.UtcNow.AddMinutes(-awayThresholdMinutes);

        // Find employees who exited but haven't returned within threshold
        var awayEmployees = await _context.EmployeeEvents
            .Where(e => e.EventType == EventType.Exit &&
                       e.EventTime <= thresholdTime &&
                       e.EventTime >= DateTime.Today)
            .GroupBy(e => e.EmployeeId)
            .Where(g => !_context.EmployeeEvents
                .Any(entry => entry.EmployeeId == g.Key &&
                             entry.EventType == EventType.Entry &&
                             entry.EventTime > g.Max(exit => exit.EventTime)))
            .Select(g => new { EmployeeId = g.Key, LastExitTime = g.Max(e => e.EventTime) })
            .ToListAsync(cancellationToken);

        foreach (var awayEmployee in awayEmployees)
        {
            // Check if alert already exists
            var existingAlert = await _context.Alerts
                .FirstOrDefaultAsync(a => a.EmployeeId == awayEmployee.EmployeeId &&
                                         a.AlertType == AlertType.Away &&
                                         a.Status == AlertStatus.Active &&
                                         a.GeneratedAt.Date == DateTime.Today, cancellationToken);

            if (existingAlert == null)
            {
                var employee = await _context.Employees.FindAsync(awayEmployee.EmployeeId);
                var alert = new AdjdAlert
                {
                    //Id = Guid.NewGuid(),
                    AlertType = AlertType.Away,
                    Status = AlertStatus.Active,
                    Title = "Employee Away Too Long",
                    Description = $"Employee {employee?.Name} ({employee?.EmployeeNumber}) has been away for more than {awayThresholdMinutes} minutes",
                    GeneratedAt = DateTime.UtcNow,
                    Priority = PriorityLevel.Low,
                    EmployeeId = awayEmployee.EmployeeId
                };

                _context.Alerts.Add(alert);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Generated away alerts for {Count} employees", awayEmployees.Count);
    }

    public async Task ResolveAlertsAsync(Guid employeeId, AlertType alertType, CancellationToken cancellationToken = default)
    {
        var activeAlerts = await _context.Alerts
            .Where(a => a.EmployeeId == employeeId &&
                       a.AlertType == alertType &&
                       a.Status == AlertStatus.Active)
            .ToListAsync(cancellationToken);

        foreach (var alert in activeAlerts)
        {
            alert.Status = AlertStatus.Resolved;
            alert.ResolvedAt = DateTime.UtcNow;
            alert.ResolvedBy = "System";
            alert.ResolutionNotes = "Automatically resolved by system event";
        }

        if (activeAlerts.Any())
        {
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Resolved {Count} {AlertType} alerts for employee {EmployeeId}",
                activeAlerts.Count, alertType, employeeId);
        }
    }
}
