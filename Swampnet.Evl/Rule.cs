using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public class Rule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public Expression Expression { get; set; }
        public ActionDefinition[] Actions { get; set; }
    }
}
