using Swampnet.Evl.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mocks
{
    class MockedEventQueueProcessor : IEventQueueProcessor
    {
        public void Enqueue(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Enqueue(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }
    }
}
