using Swampnet.Evl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;
using System.Collections.Concurrent;

namespace Swampnet.Evl.Services
{
    class EventProcessor : IEventProcessor
    {
        private readonly ConcurrentQueue<Event> _queue = new ConcurrentQueue<Event>();

        public EventProcessor()
        {

        }

        public void Enqueue(Event evt)
        {
            _queue.Enqueue(evt);
        }

        public void Enqueue(IEnumerable<Event> evts)
        {
            foreach(var evt in evts)
            {
                Enqueue(evt);
            }
        }
    }
}
