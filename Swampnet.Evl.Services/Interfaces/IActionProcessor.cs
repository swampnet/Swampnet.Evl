using Swampnet.Evl.Services.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Interfaces
{
    interface IActionProcessor
    {
        string Name { get; }
        Task ApplyAsync(EventsContext context, EventEntity evt, ActionDefinition definition);
    }
}
