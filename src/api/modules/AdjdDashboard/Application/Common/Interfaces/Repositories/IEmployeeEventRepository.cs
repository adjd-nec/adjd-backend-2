using FSH.Starter.AdjdDashboard.Domain;

namespace FSH.Starter.AdjdDashboard.Application.Common.Interfaces;

public interface IEmployeeEventRepository : IRepository<AdjdEmployeeEvent, Guid>
{
    Task<IEnumerable<AdjdEmployeeEvent>> GetByEmployeeAsync(Guid employeeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdEmployeeEvent>> GetByCameraAsync(Guid cameraId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdEmployeeEvent>> GetUnprocessedEventsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdEmployeeEvent>> GetTodaysEventsAsync(CancellationToken cancellationToken = default);
    Task<AdjdEmployeeEvent?> GetLastEventForEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<bool> HasEmployeeEnteredTodayAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<bool> HasEmployeeExitedTodayAsync(Guid employeeId, CancellationToken cancellationToken = default);
}
