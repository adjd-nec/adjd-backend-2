using FSH.Starter.AdjdDashboard.Domain;

namespace FSH.Starter.AdjdDashboard.Application.Common.Interfaces;

public interface IEmployeeRepository : IRepository<AdjdEmployee, Guid>
{
    Task<AdjdEmployee?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default);
    Task<AdjdEmployee?> GetByNeoFaceIdAsync(string neoFacePersonId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdEmployee>> GetByLocationAsync(Guid locationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdEmployee>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdEmployee>> GetActiveEmployeesAsync(CancellationToken cancellationToken = default);
}
