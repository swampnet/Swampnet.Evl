using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.Actions
{
    class ChangeCategoryActionHandler : IActionHandler
    {
        public Task ApplyAsync(Event evt, ActionDefinition actionDefinition, Rule rule)
        {
            var cat = actionDefinition.Properties.StringValue("category");
            if(!string.IsNullOrEmpty(cat))
            {
                evt.Category = Enum.Parse<EventCategory>(cat, true);
            }

            return Task.CompletedTask;
        }
    }
}
