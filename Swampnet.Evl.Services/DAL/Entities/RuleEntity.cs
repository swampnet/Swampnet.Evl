using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Services.DAL
{
    class RuleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool IsEnabled { get; set; }
        public string Expression { get; set; }
        public string Actions { get; set; }
    }
}
