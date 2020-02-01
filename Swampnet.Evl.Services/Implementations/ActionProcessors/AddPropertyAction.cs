using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services.Implementations.ActionProcessors
{
    class AddPropertyAction : IActionProcessor
    {
        public string Name => "add-property";

        public Task ApplyAsync(EventsContext context, EventEntity evt, ActionDefinition definition)
        {
            foreach(var p in definition.Properties)
            {
                evt.Properties.Add(new EventPropertyEntity() {
                    Category = p.Category,
                    Name = p.Name,
                    Value = p.Value
                });
            }

            return Task.CompletedTask;
        }
    }
}
