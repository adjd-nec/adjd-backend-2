using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;
using FSH.Starter.AdjdDashboard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdjdDashboardModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Database configuration
        services.AddAdjdDashboardDatabase(configuration);

        // Repository registrations
        services.AddScoped<ICameraRepository, CameraRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IWatchListRepository, WatchListRepository>();
        services.AddScoped<IEmployeeEventRepository, EmployeeEventRepository>();
        services.AddScoped<IAlertRepository, AlertRepository>();

        // Service registrations
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }

    private static IServiceCollection AddAdjdDashboardDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Main dashboard database
        var dashboardConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? configuration.GetValue<string>("DatabaseOptions:ConnectionString");

        services.AddDbContext<AdjdDashboardDbContext>(options =>
        {
            options.UseSqlServer(dashboardConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

            // Enable sensitive data logging in development
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // NeoFace Watch read-only database
        services.Configure<NeoFaceWatchOptions>(configuration.GetSection("NeoFaceWatchOptions"));

        services.AddDbContext<NeoFaceWatchDbContext>((serviceProvider, options) =>
        {
            var nfwOptions = serviceProvider.GetRequiredService<IOptions<NeoFaceWatchOptions>>().Value;
            options.UseSqlServer(nfwOptions.ConnectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout(30);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

            // Optimize for read-only access
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}
