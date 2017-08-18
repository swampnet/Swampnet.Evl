using Swampnet.Evl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;

namespace Swampnet.Evl.Services
{
    class DummyEventProcessor : IEventProcessor
    {
        public int Priority => 0;

        public void Process(Event evt)
        {
            //Log.Information("Processing evt {EventSummary} {Processor}", evt.Summary, this.GetType().Name);
        }
    }

    class AnotherDummyEventProcessor : IEventProcessor
    {
        public int Priority => 0;

        public void Process(Event evt)
        {
            //Log.Information("Processing evt {EventSummary} {Processor}", evt.Summary, this.GetType().Name);
        }
    }
}
