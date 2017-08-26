using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.Actions
{
    class AddPropertyActionHandler : IActionHandler
    {
        public void Apply(Event evt, ActionDefinition actionDefinition, Rule rule)
        {
            if(actionDefinition.Properties != null && actionDefinition.Properties.Any())
            {
                if (evt.Properties == null)
                {
                    evt.Properties = new List<Property>();
                }

                evt.Properties.AddRange(actionDefinition.Properties.Select(p => new Property(p.Category, p.Name, p.Value)));
            }
        }
    }
}
