using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Entities
{
    public class ActionDefinition
    {
        public ActionDefinition()
        {
        }

        public ActionDefinition(string type)
            : this()
        {
            Type = type;
        }

        public string Type { get; set; }

        public Property[] Properties { get; set; }
    }
}
