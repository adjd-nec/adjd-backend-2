using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : Repository<AdjdEmployee, Guid>, IEmployeeRepository
{
    public EmployeeRepository(AdjdDashboardDbContext context) : base(context) { }

    public override async Task<AdjdEmployee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.PrimaryLocation)
            .Include(e => e.WorkSchedule)
                .ThenInclude(ws => ws!.WatchList)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<AdjdEmployee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.PrimaryLocation)
            .Include(e => e.WorkSchedule)
            .ToListAsync(cancellationToken);
    }

    public async Task<AdjdEmployee?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.PrimaryLocation)
            .Include(e => e.WorkSchedule)
            .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber, cancellationToken);
    }

    public async Task<AdjdEmployee?> GetByNeoFaceIdAsync(string neoFacePersonId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.PrimaryLocation)
            .Include(e => e.WorkSchedule)
            .FirstOrDefaultAsync(e => e.NeoFacePersonId == neoFacePersonId, cancellationToken);
    }

    public async Task<IEnumerable<AdjdEmployee>> GetByLocationAsync(Guid locationId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.WorkSchedule)
            .Where(e => e.PrimaryLocationId == locationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdEmployee>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.PrimaryLocation)
            .Where(e => e.Department == department)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdEmployee>> GetActiveEmployeesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.PrimaryLocation)
            .Include(e => e.WorkSchedule)
            .Where(e => e.IsActive && (e.TerminationDate == null || e.TerminationDate > DateTime.Now))
            .ToListAsync(cancellationToken);
    }
}
