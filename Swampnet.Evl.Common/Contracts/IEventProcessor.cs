using Swampnet.Evl.Client;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IEventProcessor
    {
        Task ProcessAsync(EventDetails evt);

        int Priority { get; }
    }
}
