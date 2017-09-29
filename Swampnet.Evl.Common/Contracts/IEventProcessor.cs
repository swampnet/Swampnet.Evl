using Swampnet.Evl.Client;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IEventProcessor
    {
        Task ProcessAsync(Event evt);

        int Priority { get; }
    }
}
