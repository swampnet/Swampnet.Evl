using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Entities
{
    public class Rule
    {
        public Rule()
        {
        }

        public Rule(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Expression Expression { get; set; }
        public ActionDefinition[] Actions { get; set; }
    }
}
