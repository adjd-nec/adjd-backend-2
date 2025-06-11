using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Core.Domain.Enums;
public enum AlertType
{
    EntryMissing = 0,
    ExitMissing = 1,
    DoubleEntry = 2,
    DoubleExit = 3,
    AwayTooLong = 4
}
