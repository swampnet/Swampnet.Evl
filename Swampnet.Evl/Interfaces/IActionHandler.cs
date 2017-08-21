using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Interfaces
{
    public interface IActionHandler
    {
        void Apply(Event evt, IEnumerable<IProperty> properties);
    }
}
