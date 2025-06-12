using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;

public class EmployeeEventRepository : Repository<AdjdEmployeeEvent, Guid>, IEmployeeEventRepository
{
    public EmployeeEventRepository(AdjdDashboardDbContext context) : base(context) { }

    public async Task<IEnumerable<AdjdEmployeeEvent>> GetByEmployeeAsync(Guid employeeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(e => e.Employee)
            .Include(e => e.Camera)
            .Where(e => e.EmployeeId == employeeId);

        if (startDate.HasValue)
            query = query.Where(e => e.EventTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.EventTime <= endDate.Value);

        return await query
            .OrderByDescending(e => e.EventTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdEmployeeEvent>> GetByCameraAsync(Guid cameraId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(e => e.Employee)
            .Include(e => e.Camera)
            .Where(e => e.CameraId == cameraId);

        if (startDate.HasValue)
            query = query.Where(e => e.EventTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.EventTime <= endDate.Value);

        return await query
            .OrderByDescending(e => e.EventTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdEmployeeEvent>> GetUnprocessedEventsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Employee)
            .Include(e => e.Camera)
            .Where(e => !e.IsProcessed)
            .OrderBy(e => e.EventTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdEmployeeEvent>> GetTodaysEventsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return await _dbSet
            .Include(e => e.Employee)
            .Include(e => e.Camera)
            .Where(e => e.EventTime >= today && e.EventTime < tomorrow)
            .OrderByDescending(e => e.EventTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<AdjdEmployeeEvent?> GetLastEventForEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Camera)
            .Where(e => e.EmployeeId == employeeId)
            .OrderByDescending(e => e.EventTime)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> HasEmployeeEnteredTodayAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return await _dbSet
            .AnyAsync(e => e.EmployeeId == employeeId &&
                          e.EventType == EventType.Entry &&
                          e.EventTime >= today &&
                          e.EventTime < tomorrow, cancellationToken);
    }

    public async Task<bool> HasEmployeeExitedTodayAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return await _dbSet
            .AnyAsync(e => e.EmployeeId == employeeId &&
                          e.EventType == EventType.Exit &&
                          e.EventTime >= today &&
                          e.EventTime < tomorrow, cancellationToken);
    }
}
