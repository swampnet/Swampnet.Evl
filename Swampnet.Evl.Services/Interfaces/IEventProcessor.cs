using Swampnet.Evl.Services.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Interfaces
{
    interface IEventProcessor
    {
        int Priority { get; }
        Task ProcessAsync(EventEntity entity);
    }
}
