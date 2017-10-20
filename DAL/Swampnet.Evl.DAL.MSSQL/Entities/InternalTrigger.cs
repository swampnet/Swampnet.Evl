using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL.Entities
{
    class InternalTrigger
    {
        public InternalTrigger()
        {
            Actions = new List<InternalAction>();
        }

        public long Id { get; set; }

        public DateTime TimestampUtc { get; set; }

        public string RuleName { get; set; }

		public Guid RuleId { get; set; }

		public ICollection<InternalAction> Actions { get; set; }

        #region FK Stuff

        public InternalEvent Event { get; set; }
        public Guid EventId { get; set; }

		#endregion
	}
}
