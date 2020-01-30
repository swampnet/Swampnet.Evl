using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Services.DAL.Entities
{
    class RuleEntity
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }

        //public string SerializedExpression { get; set; }
        //public string SerializedActions { get; set; }
        

        public Expression Expression { get; set; }
        public ActionDefinition[] Actions { get; set; }
    }
}
