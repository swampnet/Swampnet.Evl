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


        public void Enqueue(Guid id)
        {
            Queue.Enqueue(id);
        }


        public void Enqueue(IEnumerable<Guid> ids)
        {
            foreach(var id in ids)
            {
                Enqueue(id);
            }
        }
    }
}
