using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Common.Entities
{
    public class Rule
    {
        /// <summary>
        /// Construction
        /// </summary>
        public Rule()
        {
			IsActive = true;
        }

        /// <summary>
        /// Construction
        /// </summary>
        public Rule(string name)
			: this()
        {
            Name = name;
        }

        /// <summary>
        /// Rule ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Active flag
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Rule name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Execution oprder
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Rule expression
        /// </summary>
        /// <remarks>
        /// If this expression evaluates to true, run any actions
        /// </remarks>
        public Expression Expression { get; set; }

        /// <summary>
        /// Actions to run if expression evaluates to true
        /// </summary>
        public ActionDefinition[] Actions { get; set; }

        /// <summary>
        /// Audit trail
        /// </summary>
        public Audit[]  Audit { get; set; }
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

        /// <summary>
        /// Execution order
        /// </summary>
        public int Order { get; set; }
        public string[] Actions { get; set; }
        public bool IsActive { get; set; }
    }


    public class RuleOrder
    {
        public Guid RuleId { get; set; }
        public int Order { get; set; }
    }
}
