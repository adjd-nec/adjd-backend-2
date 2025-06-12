using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;

namespace FSH.Starter.AdjdDashboard.Application.Common.Interfaces;

public interface IAlertRepository : IRepository<AdjdAlert, Guid>
{
    Task<IEnumerable<AdjdAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdAlert>> GetByTypeAsync(AlertType alertType, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdAlert>> GetByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdAlert>> GetTodaysAlertsAsync(CancellationToken cancellationToken = default);
    Task<int> GetActiveAlertCountAsync(AlertType? alertType = null, CancellationToken cancellationToken = default);
}
