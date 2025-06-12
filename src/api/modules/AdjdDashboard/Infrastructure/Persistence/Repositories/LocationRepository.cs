using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;

public class LocationRepository : Repository<AdjdLocation, Guid>, ILocationRepository
{
    public LocationRepository(AdjdDashboardDbContext context) : base(context) { }

    public async Task<IEnumerable<AdjdLocation>> GetByBuildingAsync(string building, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(l => l.Building == building)
            .ToListAsync(cancellationToken);
    }

    public async Task<AdjdLocation?> GetWithCamerasAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Cameras)
                .ThenInclude(c => c.Configuration)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<AdjdLocation?> GetWithEmployeesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Employees)
                .ThenInclude(e => e.WorkSchedule)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
}
