//using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;


//namespace FSH.Starter.AdjdDashboard.Infrastructure.Extensions;

//public static class DatabaseExtensions
//{
//    public static async Task<WebApplication> EnsureAdjdDashboardDatabaseAsync(this WebApplication app)
//    {
//        using var scope = app.Services.CreateScope();
//        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

//        try
//        {
//            var dashboardContext = scope.ServiceProvider.GetRequiredService<AdjdDashboardDbContext>();

//            // Ensure database is created and migrations are applied
//            if (app.Environment.IsDevelopment())
//            {
//                await dashboardContext.Database.EnsureCreatedAsync();

//                // Apply any pending migrations
//                if ((await dashboardContext.Database.GetPendingMigrationsAsync()).Any())
//                {
//                    logger.LogInformation("Applying pending migrations for ADJD Dashboard database...");
//                    await dashboardContext.Database.MigrateAsync();
//                }
//            }

//            // Test NeoFace Watch connection
//            var neoFaceContext = scope.ServiceProvider.GetRequiredService<NeoFaceWatchDbContext>();
//            if (await neoFaceContext.Database.CanConnectAsync())
//            {
//                logger.LogInformation("Successfully connected to NeoFace Watch database");
//            }
//            else
//            {
//                logger.LogWarning("Could not connect to NeoFace Watch database");
//            }

//            logger.LogInformation("ADJD Dashboard database initialization completed successfully");
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, "An error occurred during ADJD Dashboard database initialization");
//            throw;
//        }

//        return app;
//    }
//}
