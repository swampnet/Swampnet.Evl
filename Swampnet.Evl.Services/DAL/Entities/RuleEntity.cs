using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Services.DAL.Entities
{
    class RuleEntity : Rule
    {
        public string SerializedExpression { get; set; }
        public string SerializedActions { get; set; }
    }
}
