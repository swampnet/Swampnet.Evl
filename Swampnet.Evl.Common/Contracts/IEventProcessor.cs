using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    // Pretty bad name, it's actually processing events (via an internal queue)
    public interface IEventProcessorQueue
    {
        void Enqueue(Event evt);
        void Enqueue(IEnumerable<Event> evts);
    }



    public interface IEventProcessor
    {
        void Process(Event evt);
        int Priority { get; }
    }
}
