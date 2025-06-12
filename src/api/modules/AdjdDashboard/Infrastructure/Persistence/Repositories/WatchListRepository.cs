using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;

public class WatchListRepository : Repository<AdjdWatchList, Guid>, IWatchListRepository
{
    public WatchListRepository(AdjdDashboardDbContext context) : base(context) { }

    public async Task<AdjdWatchList?> GetDefaultWatchListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(wl => wl.WorkSchedules)
            .FirstOrDefaultAsync(wl => wl.IsDefault, cancellationToken);
    }

    public async Task<AdjdWatchList?> GetByNeoFaceIdAsync(string neoFaceWatchListId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(wl => wl.WorkSchedules)
            .FirstOrDefaultAsync(wl => wl.NeoFaceWatchListId == neoFaceWatchListId, cancellationToken);
    }

    public async Task<IEnumerable<AdjdWatchList>> GetActiveWatchListsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(wl => wl.WorkSchedules)
            .Where(wl => wl.IsActive)
            .ToListAsync(cancellationToken);
    }
}
