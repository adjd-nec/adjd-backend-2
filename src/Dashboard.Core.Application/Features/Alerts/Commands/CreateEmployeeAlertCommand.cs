using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.Contracts;
using Dashboard.Core.Domain.Entities;
using Dashboard.Core.Domain.Enums;
using MediatR;

namespace Dashboard.Core.Application.Features.Alerts.Commands;
public record CreateEmployeeAlertCommand(
    Guid EmployeeId,
    string EmployeeName,
    AlertType AlertType,
    DateTime AlertDateTime,
    string Description,
    Guid LocationId,
    Guid? RelatedEventId = null) : IRequest<Guid>;

public class CreateEmployeeAlertCommandHandler : IRequestHandler<CreateEmployeeAlertCommand, Guid>
{
    private readonly IDashboardDbContext _context;

    public CreateEmployeeAlertCommandHandler(IDashboardDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateEmployeeAlertCommand request, CancellationToken cancellationToken)
    {
        var alert = new EmployeeAlert(
            request.EmployeeId,
            request.EmployeeName,
            request.AlertType,
            request.AlertDateTime,
            request.Description,
            request.LocationId,
            request.RelatedEventId);

        _context.EmployeeAlerts.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);

        return alert.Id;
    }
}
