using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.AdjdDashboard.Application.Common.DTOs.Requests;
using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Infrastructure.Extensions;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;
using FSH.Starter.AdjdDashboard.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.AdjdDashboard;

public static class AdjdDashboardModule
{
    public class Endpoints : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var dashboardGroup = app.MapGroup("adjd").WithTags("ADJD Dashboard");

            // Dashboard endpoints
            dashboardGroup.MapGet("/dashboard/summary", GetDashboardSummary)
                .WithName("GetDashboardSummary")
                .WithSummary("Get dashboard summary statistics")
                .Produces<DashboardSummaryResponse>();

            dashboardGroup.MapGet("/dashboard/employees/status", GetEmployeeStatus)
                .WithName("GetEmployeeStatus")
                .WithSummary("Get current employee status")
                .Produces<IEnumerable<EmployeeStatusResponse>>();

            dashboardGroup.MapGet("/dashboard/cameras/status", GetCameraStatus)
                .WithName("GetCameraStatus")
                .WithSummary("Get camera status")
                .Produces<IEnumerable<CameraStatusResponse>>();

            dashboardGroup.MapGet("/dashboard/alerts/active", GetActiveAlerts)
                .WithName("GetActiveAlerts")
                .WithSummary("Get active alerts")
                .Produces<IEnumerable<AlertSummaryResponse>>();

            // Location endpoints
            var locationsGroup = dashboardGroup.MapGroup("/locations").WithTags("Locations");
            locationsGroup.MapGet("/", GetLocations)
                .WithName("GetLocations")
                .WithSummary("Get all locations");

            locationsGroup.MapPost("/", CreateLocation)
                .WithName("CreateLocation")
                .WithSummary("Create a new location");

            // Employee endpoints  
            var employeesGroup = dashboardGroup.MapGroup("/employees").WithTags("Employees");
            employeesGroup.MapGet("/", GetEmployees)
                .WithName("GetEmployees")
                .WithSummary("Get all employees");

            // Camera endpoints
            var camerasGroup = dashboardGroup.MapGroup("/cameras").WithTags("Cameras");
            camerasGroup.MapGet("/", GetCameras)
                .WithName("GetCameras")
                .WithSummary("Get all cameras");

            // Alert endpoints
            var alertsGroup = dashboardGroup.MapGroup("/alerts").WithTags("Alerts");
            alertsGroup.MapGet("/", GetAlerts)
                .WithName("GetAlerts")
                .WithSummary("Get alerts");
        }

        private static async Task<IResult> GetDashboardSummary(IDashboardService dashboardService, CancellationToken cancellationToken)
        {
            var summary = await dashboardService.GetDashboardSummaryAsync(cancellationToken);
            return Results.Ok(new DashboardSummaryResponse
            {
                TotalEmployeesPresent = summary.TotalEmployeesPresent,
                TotalCameras = summary.TotalCameras,
                ActiveCameras = summary.ActiveCameras,
                InactiveCameras = summary.InactiveCameras,
                TotalActiveAlerts = summary.TotalActiveAlerts,
                TodayEntries = summary.TodayEntries,
                TodayExits = summary.TodayExits,
                AlertCounts = summary.AlertCounts
            });
        }

        private static async Task<IResult> GetEmployeeStatus(IDashboardService dashboardService, CancellationToken cancellationToken)
        {
            var employeeStatus = await dashboardService.GetCurrentEmployeeStatusAsync(cancellationToken);
            var response = employeeStatus.Select(e => new EmployeeStatusResponse
            {
                EmployeeId = e.EmployeeId,
                Name = e.EmployeeName,
                EmployeeNumber = e.EmployeeNumber,
                Status = e.Status,
                LastSeen = e.LastSeen,
                Location = e.Location
            });
            return Results.Ok(response);
        }

        private static async Task<IResult> GetCameraStatus(IDashboardService dashboardService, CancellationToken cancellationToken)
        {
            var cameraStatus = await dashboardService.GetCameraStatusAsync(cancellationToken);
            var response = cameraStatus.Select(c => new CameraStatusResponse
            {
                CameraId = c.CameraId,
                Name = c.CameraName,
                IsOperational = c.IsOperational,
                Location = c.LocationName,
                CameraType = c.CameraType,
                LastActivity = c.LastEventTime
            });
            return Results.Ok(response);
        }

        private static async Task<IResult> GetActiveAlerts(IDashboardService dashboardService, CancellationToken cancellationToken)
        {
            var alerts = await dashboardService.GetActiveAlertsAsync(cancellationToken);
            var response = alerts.Select(a => new AlertSummaryResponse
            {
                AlertId = a.AlertId,
                AlertType = a.AlertType,
                Title = a.Title,
                Description = a.Description,
                GeneratedAt = a.GeneratedAt,
                Priority = a.Priority,
                EmployeeName = a.EmployeeName
            });
            return Results.Ok(response);
        }

        private static async Task<IResult> GetLocations(ILocationRepository locationRepository, CancellationToken cancellationToken)
        {
            var locations = await locationRepository.GetAllAsync(cancellationToken);
            var response = locations.Select(l => new LocationResponse
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Building = l.Building,
                Floor = l.Floor,
                Zone = l.Zone
            });
            return Results.Ok(response);
        }

        private static async Task<IResult> CreateLocation(CreateLocationRequest request, ILocationRepository locationRepository, CancellationToken cancellationToken)
        {
            var location = new AdjdLocation
            {
                Name = request.Name,
                Description = request.Description,
                Building = request.Building,
                Floor = request.Floor,
                Zone = request.Zone,
                FloorPlanFileName = request.FloorPlanFileName,
                FloorPlanContentType = request.FloorPlanContentType,
                FloorPlanImage = request.FloorPlanImage
            };

            var result = await locationRepository.AddAsync(location, cancellationToken);

            return Results.Created($"/adjd/locations/{result.Id}", new LocationResponse
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Building = result.Building,
                Floor = result.Floor,
                Zone = result.Zone
            });
        }

        private static async Task<IResult> GetEmployees(IEmployeeRepository employeeRepository, CancellationToken cancellationToken)
        {
            var employees = await employeeRepository.GetAllAsync(cancellationToken);
            var response = employees.Select(e => new EmployeeResponse
            {
                Id = e.Id,
                Name = e.Name,
                EmployeeNumber = e.EmployeeNumber,
                Department = e.Department,
                Position = e.Position,
                Email = e.Email,
                IsActive = e.IsActive
            });
            return Results.Ok(response);
        }

        private static async Task<IResult> GetCameras(ICameraRepository cameraRepository, CancellationToken cancellationToken)
        {
            var cameras = await cameraRepository.GetAllAsync(cancellationToken);
            var response = cameras.Select(c => new CameraResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CameraType = c.CameraType.ToString(),
                IsOperational = c.IsOperational,
                LocationName = c.Location?.Name
            });
            return Results.Ok(response);
        }

        private static async Task<IResult> GetAlerts(IAlertRepository alertRepository, CancellationToken cancellationToken)
        {
            var alerts = await alertRepository.GetActiveAlertsAsync(cancellationToken);
            var response = alerts.Select(a => new AlertResponse
            {
                Id = a.Id,
                AlertType = a.AlertType.ToString(),
                Status = a.Status.ToString(),
                Title = a.Title,
                Description = a.Description,
                GeneratedAt = a.GeneratedAt,
                Priority = a.Priority.ToString()
            });
            return Results.Ok(response);
        }
    }

    public static WebApplicationBuilder RegisterAdjdDashboardServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Add ADJD Dashboard module services
        builder.Services.AddAdjdDashboardModule(builder.Configuration);

        // Bind the database context
        builder.Services.BindDbContext<AdjdDashboardDbContext>();

        return builder;
    }

    public static WebApplication UseAdjdDashboardModule(this WebApplication app)
    {
        return app;
    }
}

// Response DTOs
public record DashboardSummaryResponse
{
    public int TotalEmployeesPresent { get; init; }
    public int TotalCameras { get; init; }
    public int ActiveCameras { get; init; }
    public int InactiveCameras { get; init; }
    public int TotalActiveAlerts { get; init; }
    public int TodayEntries { get; init; }
    public int TodayExits { get; init; }
    public object? AlertCounts { get; init; }
}

public record EmployeeStatusResponse
{
    public Guid EmployeeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string EmployeeNumber { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime? LastSeen { get; init; }
    public string? Location { get; init; }
}

public record CameraStatusResponse
{
    public Guid CameraId { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsOperational { get; init; }
    public string? Location { get; init; }
    public string CameraType { get; init; } = string.Empty;
    public DateTime? LastActivity { get; init; }
}

public record AlertSummaryResponse
{
    public Guid AlertId { get; init; }
    public string AlertType { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime GeneratedAt { get; init; }
    public string Priority { get; init; } = string.Empty;
    public string? EmployeeName { get; init; }
}

public record LocationResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Building { get; init; } = string.Empty;
    public string? Floor { get; init; }
    public string? Zone { get; init; }
}

public record EmployeeResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string EmployeeNumber { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string? Position { get; init; }
    public string? Email { get; init; }
    public bool IsActive { get; init; }
}

public record CameraResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string CameraType { get; init; } = string.Empty;
    public bool IsOperational { get; init; }
    public string? LocationName { get; init; }
}

public record AlertResponse
{
    public Guid Id { get; init; }
    public string AlertType { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime GeneratedAt { get; init; }
    public string Priority { get; init; } = string.Empty;
}
