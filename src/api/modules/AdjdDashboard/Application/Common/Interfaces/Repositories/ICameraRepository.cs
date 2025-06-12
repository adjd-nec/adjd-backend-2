using FSH.Starter.AdjdDashboard.Domain;
using FSH.Starter.AdjdDashboard.Domain.Enums;

namespace FSH.Starter.AdjdDashboard.Application.Common.Interfaces;

public interface ICameraRepository : IRepository<AdjdCamera, Guid>
{
    Task<IEnumerable<AdjdCamera>> GetByLocationAsync(Guid locationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdCamera>> GetByTypeAsync(CameraType cameraType, CancellationToken cancellationToken = default);
    Task<AdjdCamera?> GetByNeoFaceIdAsync(string neoFaceCameraId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdjdCamera>> GetOperationalCamerasAsync(CancellationToken cancellationToken = default);
}
