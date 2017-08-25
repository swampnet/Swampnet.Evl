using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Client;
using Serilog;

namespace Swampnet.Evl.EventProcessors
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
