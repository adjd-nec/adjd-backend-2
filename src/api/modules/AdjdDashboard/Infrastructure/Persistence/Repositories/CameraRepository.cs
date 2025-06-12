using FSH.Starter.AdjdDashboard.Application.Common.Interfaces;
using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;
using FSH.Starter.AdjdDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.AdjdDashboard.Infrastructure.Persistence.Repositories;

public class CameraRepository : Repository<AdjdCamera, Guid>, ICameraRepository
{
    public CameraRepository(AdjdDashboardDbContext context) : base(context) { }

    public override async Task<AdjdCamera?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Location)
            .Include(c => c.Configuration)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<AdjdCamera>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Location)
            .Include(c => c.Configuration)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdCamera>> GetByLocationAsync(Guid locationId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Location)
            .Include(c => c.Configuration)
            .Where(c => c.LocationId == locationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdjdCamera>> GetByTypeAsync(CameraType cameraType, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Location)
            .Where(c => c.CameraType == cameraType)
            .ToListAsync(cancellationToken);
    }

    public async Task<AdjdCamera?> GetByNeoFaceIdAsync(string neoFaceCameraId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Location)
            .Include(c => c.Configuration)
            .FirstOrDefaultAsync(c => c.NeoFaceCameraId == neoFaceCameraId, cancellationToken);
    }

    public async Task<IEnumerable<AdjdCamera>> GetOperationalCamerasAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Location)
            .Where(c => c.IsOperational)
            .ToListAsync(cancellationToken);
    }
}
