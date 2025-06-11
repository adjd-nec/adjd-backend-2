using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.Contracts;
using Dashboard.Core.Domain.Entities;
using Dashboard.Core.Domain.Enums;
using MediatR;

namespace Dashboard.Core.Application.Features.EmployeeEvents.Commands
{
    public record CreateEmployeeEventCommand(
     Guid EmployeeId,
     string EmployeeName,
     EventType EventType,
     Guid CameraId,
     Guid LocationId,
     DateTime EventDateTime,
     decimal? ConfidenceScore = null,
     Guid? NeoFaceMatchId = null) : IRequest<Guid>;

    public class CreateEmployeeEventCommandHandler : IRequestHandler<CreateEmployeeEventCommand, Guid>
    {
        private readonly IDashboardDbContext _context;

        public CreateEmployeeEventCommandHandler(IDashboardDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateEmployeeEventCommand request, CancellationToken cancellationToken)
        {
            var employeeEvent = new EmployeeEvent(
                request.EmployeeId,
                request.EmployeeName,
                request.EventType,
                request.CameraId,
                request.LocationId,
                request.EventDateTime,
                request.ConfidenceScore,
                request.NeoFaceMatchId);

            _context.EmployeeEvents.Add(employeeEvent);
            await _context.SaveChangesAsync(cancellationToken);

            return employeeEvent.Id;
        }
    }
}
