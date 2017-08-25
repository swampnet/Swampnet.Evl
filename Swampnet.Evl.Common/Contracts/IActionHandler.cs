using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Contracts
{
    public interface IActionHandler
    {
        void Apply(Event evt, IEnumerable<IProperty> properties);
    }
}
