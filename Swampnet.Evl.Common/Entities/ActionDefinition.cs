using Swampnet.Evl.Client;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Entities
{
    public class ActionDefinition
    {
        public ActionDefinition()
        {
        }

        public ActionDefinition(string type, IEnumerable<Property> properties = null)
            : this()
        {
            Type = type;
            Properties = properties?.ToArray();
        }

        public string Type { get; set; }

        public Property[] Properties { get; set; }
    }
}
