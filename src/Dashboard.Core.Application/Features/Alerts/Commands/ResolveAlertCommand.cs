using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Core.Application.Features.Alerts.Commands;
public record ResolveAlertCommand(Guid AlertId, Guid ResolvedBy) : IRequest;

public class ResolveAlertCommandHandler : IRequestHandler<ResolveAlertCommand>
{
    private readonly IDashboardDbContext _context;

    public ResolveAlertCommandHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ResolveAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = await _context.EmployeeAlerts
            .FirstOrDefaultAsync(a => a.Id == request.AlertId && !a.IsDeleted, cancellationToken);

        if (alert == null)
            throw new InvalidOperationException($"Alert with ID {request.AlertId} not found");

        alert.Resolve(request.ResolvedBy);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
