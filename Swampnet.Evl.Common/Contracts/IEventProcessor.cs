using Swampnet.Evl.Client;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IEventQueueProcessor
    {
        void Enqueue(Guid id);
        void Enqueue(IEnumerable<Guid> ids);
    }



    public interface IEventProcessor
    {
        void Process(Event evt);
        int Priority { get; }
    }
}
