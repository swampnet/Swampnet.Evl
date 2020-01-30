using System;
using System.Collections.Generic;
using System.Linq;

namespace Swampnet.Evl
{
    public class ActionDefinition
    {
        public ActionDefinition()
        {
            IsActive = true;
        }

        public ActionDefinition(string type, IEnumerable<Property> properties = null)
            : this()
        {
            Type = type;
            Properties = properties?.ToArray();
        }


        public bool IsActive { get; set; }
        public string Type { get; set; }

        public Property[] Properties { get; set; }

        public override string ToString()
        {
            return $"{Type}" + (IsActive ? "" : " (disabled)");
        }
    }
}
