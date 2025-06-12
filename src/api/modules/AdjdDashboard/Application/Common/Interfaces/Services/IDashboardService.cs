using FSH.Starter.AdjdDashboard.Application.Common.DTOs;

namespace FSH.Starter.AdjdDashboard.Application.Common.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<EmployeeStatusDto>> GetCurrentEmployeeStatusAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CameraStatusDto>> GetCameraStatusAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AlertSummaryDto>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);
}
