using FSH.Starter.AdjdDashboard.Domain;

namespace FSH.Starter.AdjdDashboard.Application.Common.Interfaces;

public interface ILocationRepository : IRepository<AdjdLocation, Guid>
{
    Task<IEnumerable<AdjdLocation>> GetByBuildingAsync(string building, CancellationToken cancellationToken = default);
    Task<AdjdLocation?> GetWithCamerasAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AdjdLocation?> GetWithEmployeesAsync(Guid id, CancellationToken cancellationToken = default);
}
