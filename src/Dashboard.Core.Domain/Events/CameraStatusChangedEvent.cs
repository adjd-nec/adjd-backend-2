using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Entities;
using Dashboard.Core.Domain.Enums;

namespace Dashboard.Core.Domain.Events;
public record CameraStatusChangedEvent(DashboardCamera Camera, CameraStatus PreviousStatus, CameraStatus NewStatus);
