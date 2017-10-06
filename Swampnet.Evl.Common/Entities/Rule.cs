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
			IsActive = true;
        }

        public Rule(string name)
			: this()
        {
            Name = name;
        }

        public Guid? Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public Expression Expression { get; set; }
        public ActionDefinition[] Actions { get; set; }
    }


    public class RuleSummary
    {
		public RuleSummary()
		{
			IsActive = true;
		}

		public RuleSummary(Guid id, string name)
			: this()
		{
			Id = id;
			Name = name;
		}

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string[] Actions { get; set; }
        public bool IsActive { get; set; }
    }
}
