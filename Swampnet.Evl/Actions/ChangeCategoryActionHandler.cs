using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;

namespace Swampnet.Evl.Actions
{
    class ChangeCategoryActionHandler : IActionHandler
    {
        public void Apply(Event evt, IEnumerable<IProperty> properties)
        {
            var cat = properties.StringValue("category");
            if(!string.IsNullOrEmpty(cat))
            {
                evt.Category = cat;
            }
        }
    }
}
