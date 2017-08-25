using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Contracts
{
    public interface IEventQueueProcessor
    {
        void Enqueue(Guid id);
        void Enqueue(IEnumerable<Guid> ids);
    }
}
