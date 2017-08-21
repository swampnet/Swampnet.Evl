using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Entities
{
    public class Rule
    {
        public Expression Expression { get; set; }
        public ActionDefinition[] Actions { get; set; }
    }
}
