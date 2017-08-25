using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;

namespace Swampnet.Evl.Actions
{
    class DebugActionHandler : IActionHandler
    {
        public void Apply(Event evt, IEnumerable<IProperty> properties)
        {
            Log.Information("Debug Action Handler");
        }
    }
}
