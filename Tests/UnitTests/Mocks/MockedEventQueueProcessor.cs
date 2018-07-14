using Swampnet.Evl.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mocks
{
    class MockedEventQueueProcessor : IEventQueueProcessor
    {
        public Queue<Guid> Queue { get; private set; }

        public MockedEventQueueProcessor()
        {
            Queue = new Queue<Guid>();
        }


        public void Enqueue(Guid id, Swampnet.Evl.Client.Event evt)
        {
            Queue.Enqueue(id);
        }


        public void Enqueue(Guid id, IEnumerable<Swampnet.Evl.Client.Event> evts)
        {
            foreach(var e in evts)
            {
                Enqueue(id, e);
            }
        }
    }
}
