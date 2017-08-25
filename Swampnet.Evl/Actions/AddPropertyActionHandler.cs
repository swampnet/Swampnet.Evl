using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Contracts;

namespace Swampnet.Evl.Actions
{
    class AddPropertyActionHandler : IActionHandler
    {
        public void Apply(Event evt, IEnumerable<IProperty> properties)
        {
            if(properties != null && properties.Any())
            {
                if (evt.Properties == null)
                {
                    evt.Properties = new List<Property>();
                }

                evt.Properties.AddRange(properties.Select(p => new Property(p.Category, p.Name, p.Value)));
            }
        }
    }
}
