using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;

public class AlertRepository : Repository<AdjdAlert, Guid>, IAlertRepository
{
    public AlertRepository(AdjdDashboardDbContext context) : base(context) { }

    public async Task<IEnumerable<AdjdAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Employee)
            .Include(a => a.Camera)
            .Where(a => a.Status == AlertStatus.Active)
            .OrderByDescending(a => a.GeneratedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdAlert>> GetByTypeAsync(AlertType alertType, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Employee)
            .Include(a => a.Camera)
            .Where(a => a.AlertType == alertType)
            .OrderByDescending(a => a.GeneratedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdAlert>> GetByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Camera)
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.GeneratedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdAlert>> GetTodaysAlertsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        return await _dbSet
            .Include(a => a.Employee)
            .Include(a => a.Camera)
            .Where(a => a.GeneratedAt >= today && a.GeneratedAt < tomorrow)
            .OrderByDescending(a => a.GeneratedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetActiveAlertCountAsync(AlertType? alertType = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(a => a.Status == AlertStatus.Active);

        if (alertType.HasValue)
            query = query.Where(a => a.AlertType == alertType.Value);

        return await query.CountAsync(cancellationToken);
    }
}
