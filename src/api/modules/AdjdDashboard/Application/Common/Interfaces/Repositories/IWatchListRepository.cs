using FSH.Starter.AdjdDashboard.Domain;

namespace FSH.Starter.AdjdDashboard.Application.Common.Interfaces;

public interface IWatchListRepository : IRepository<AdjdWatchList, Guid>
{
    Task<AdjdWatchList?> GetDefaultWatchListAsync(CancellationToken cancellationToken = default);
    Task<AdjdWatchList?> GetByNeoFaceIdAsync(string neoFaceWatchListId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdWatchList>> GetActiveWatchListsAsync(CancellationToken cancellationToken = default);
}
