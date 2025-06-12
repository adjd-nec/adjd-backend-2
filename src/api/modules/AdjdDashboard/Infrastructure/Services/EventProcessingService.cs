using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Services;

public class EventProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventProcessingService> _logger;
    private readonly EventProcessingOptions _options;

    public EventProcessingService(
        IServiceProvider serviceProvider,
        ILogger<EventProcessingService> logger,
        IOptions<EventProcessingOptions> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Event Processing Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                await ProcessNeoFaceWatchEvents(scope, stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(_options.PollingIntervalSeconds), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Event Processing Service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during event processing");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Wait before retry
            }
        }
    }

    private async Task ProcessNeoFaceWatchEvents(IServiceScope scope, CancellationToken cancellationToken)
    {
        var neoFaceContext = scope.ServiceProvider.GetRequiredService<NeoFaceWatchDbContext>();
        var dashboardContext = scope.ServiceProvider.GetRequiredService<AdjdDashboardDbContext>();
        var cameraRepository = scope.ServiceProvider.GetRequiredService<ICameraRepository>();
        var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        var alertGenerationService = scope.ServiceProvider.GetRequiredService<IAlertGenerationService>();

        try
        {
            // Get the last processed event timestamp
            var lastProcessedTime = await GetLastProcessedTimestamp(dashboardContext, cancellationToken);

            // Get new monitoring results from NeoFace Watch
            var newResults = await neoFaceContext.MonitoringResults
                .Where(mr => mr.MatchDateTime > lastProcessedTime)
                .OrderBy(mr => mr.MatchDateTime)
                .Take(_options.MaxEventsPerBatch)
                .ToListAsync(cancellationToken);

            if (!newResults.Any())
            {
                _logger.LogDebug("No new events to process");
                return;
            }

            _logger.LogInformation("Processing {Count} new events", newResults.Count);

            foreach (var result in newResults)
            {
                await ProcessSingleEvent(result, cameraRepository, employeeRepository, dashboardContext, alertGenerationService, cancellationToken);
            }

            // Update last processed timestamp
            await UpdateLastProcessedTimestamp(dashboardContext, newResults.Max(r => r.MatchDateTime), cancellationToken);

            _logger.LogInformation("Successfully processed {Count} events", newResults.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing NeoFace Watch events");
            throw;
        }
    }

    private async Task ProcessSingleEvent(
        NfwMonitoringResult result,
        ICameraRepository cameraRepository,
        IEmployeeRepository employeeRepository,
        AdjdDashboardDbContext dashboardContext,
        IAlertGenerationService alertGenerationService,
        CancellationToken cancellationToken)
    {
        try
        {
            // Find corresponding camera and employee in our system
            var camera = await cameraRepository.GetByNeoFaceIdAsync(result.CameraID.ToString(), cancellationToken);
            var employee = await employeeRepository.GetByNeoFaceIdAsync(result.PersonID.ToString(), cancellationToken);

            if (camera == null)
            {
                _logger.LogWarning("Camera not found for NeoFace ID: {CameraId}", result.CameraID);
                return;
            }

            if (employee == null)
            {
                _logger.LogWarning("Employee not found for NeoFace ID: {PersonId}", result.PersonID);
                return;
            }

            // Check if confidence score meets threshold
            var confidenceThreshold = camera.Configuration?.MinConfidenceThreshold ?? 0.7m;
            if ((double)result.MatchScore < (double)confidenceThreshold)
            {
                _logger.LogDebug("Event rejected due to low confidence: {Score} < {Threshold}", result.MatchScore, confidenceThreshold);
                return;
            }

            // Determine event type based on camera configuration
            var eventType = DetermineEventType(camera, result);

            // Check for duplicate events
            if (await IsDuplicateEvent(dashboardContext, employee.Id, camera.Id, result.MatchDateTime, eventType, cancellationToken))
            {
                _logger.LogDebug("Duplicate event detected and skipped");
                return;
            }

            // Create employee event
            var employeeEvent = new AdjdEmployeeEvent
            {
                //Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                CameraId = camera.Id,
                EventTime = result.MatchDateTime,
                EventType = eventType,
                NeoFaceEventId = result.MatchID.ToString(),
                ConfidenceScore = (decimal)result.MatchScore,
                IsProcessed = false,
                IsValidated = true
            };

            dashboardContext.EmployeeEvents.Add(employeeEvent);
            await dashboardContext.SaveChangesAsync(cancellationToken);

            // Generate alerts based on business rules
            await alertGenerationService.ProcessEventForAlertsAsync(employeeEvent, cancellationToken);

            // Mark event as processed
            employeeEvent.IsProcessed = true;
            employeeEvent.ProcessedAt = DateTime.UtcNow;
            await dashboardContext.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("Processed event: {EventType} for employee {EmployeeId} at camera {CameraId}",
                eventType, employee.EmployeeNumber, camera.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing single event: {EventId}", result.MatchID);
            throw;
        }
    }

    private static EventType DetermineEventType(AdjdCamera camera, NfwMonitoringResult result)
    {
        // For Step 1, use simple logic based on camera type
        // In Step 2, this can be enhanced with direction detection, AI analysis, etc.
        return camera.CameraType switch
        {
            CameraType.Entry => EventType.Entry,
            CameraType.Exit => EventType.Exit,
            CameraType.Bidirectional => EventType.Unknown, // Will need additional logic
            CameraType.MonitorOnly => EventType.Unknown,
            _ => EventType.Unknown
        };
    }

    private async Task<bool> IsDuplicateEvent(
        AdjdDashboardDbContext context,
        Guid employeeId,
        Guid cameraId,
        DateTime eventTime,
        EventType eventType,
        CancellationToken cancellationToken)
    {
        var duplicateWindow = TimeSpan.FromMinutes(_options.DuplicateDetectionWindowMinutes);
        var windowStart = eventTime.Subtract(duplicateWindow);
        var windowEnd = eventTime.Add(duplicateWindow);

        return await context.EmployeeEvents
            .AnyAsync(e => e.EmployeeId == employeeId &&
                          e.CameraId == cameraId &&
                          e.EventType == eventType &&
                          e.EventTime >= windowStart &&
                          e.EventTime <= windowEnd, cancellationToken);
    }

    private async Task<DateTime> GetLastProcessedTimestamp(AdjdDashboardDbContext context, CancellationToken cancellationToken)
    {
        var config = await context.SystemConfigurations
            .FirstOrDefaultAsync(c => c.ConfigKey == "EventProcessing.LastProcessedTimestamp", cancellationToken);

        if (config != null && DateTime.TryParse(config.ConfigValue, out var timestamp))
        {
            return timestamp;
        }

        // Default to 24 hours ago for first run
        return DateTime.UtcNow.AddHours(-24);
    }

    private async Task UpdateLastProcessedTimestamp(AdjdDashboardDbContext context, DateTime timestamp, CancellationToken cancellationToken)
    {
        var config = await context.SystemConfigurations
            .FirstOrDefaultAsync(c => c.ConfigKey == "EventProcessing.LastProcessedTimestamp", cancellationToken);

        if (config == null)
        {
            config = new AdjdSystemConfiguration
            {
                //Id = Guid.NewGuid(),
                ConfigKey = "EventProcessing.LastProcessedTimestamp",
                ConfigValue = timestamp.ToString("O"),
                Description = "Last processed event timestamp from NeoFace Watch",
                ConfigType = "DateTime",
                IsSystem = true,
                Category = "EventProcessing"
            };
            context.SystemConfigurations.Add(config);
        }
        else
        {
            config.ConfigValue = timestamp.ToString("O");
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}

public class EventProcessingOptions
{
    public int PollingIntervalSeconds { get; set; } = 30;
    public int MaxEventsPerBatch { get; set; } = 100;
    public int DuplicateDetectionWindowMinutes { get; set; } = 5;
    public decimal DefaultConfidenceThreshold { get; set; } = 0.7m;
}
