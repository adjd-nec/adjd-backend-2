using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Entities;

namespace Dashboard.Core.Domain.Events;
public record EmployeeStatusUpdatedEvent(EmployeeCurrentStatus Status, EmployeeEvent Event);

