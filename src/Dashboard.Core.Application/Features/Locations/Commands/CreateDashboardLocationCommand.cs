using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.Contracts;
using Dashboard.Core.Domain.Entities;
using MediatR;

namespace Dashboard.Core.Application.Features.Locations.Commands;
public record CreateDashboardLocationCommand(
    string Name,
    string? Description = null,
    string? Address = null,
    TimeSpan? WorkingHoursStart = null,
    TimeSpan? WorkingHoursEnd = null,
    int AwayAlertMinutes = 60) : IRequest<Guid>;

public class CreateDashboardLocationCommandHandler : IRequestHandler<CreateDashboardLocationCommand, Guid>
{
    private readonly IDashboardDbContext _context;

    public CreateDashboardLocationCommandHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDashboardLocationCommand request, CancellationToken cancellationToken)
    {
        var location = new DashboardLocation(
            request.Name,
            request.Description,
            request.Address,
            request.WorkingHoursStart,
            request.WorkingHoursEnd,
            request.AwayAlertMinutes);

        _context.DashboardLocations.Add(location);
        await _context.SaveChangesAsync(cancellationToken);

        return location.Id;
    }
}
