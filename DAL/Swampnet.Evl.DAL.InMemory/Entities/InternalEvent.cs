using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory.Entities
{
    class InternalEvent
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Event timestamp (UTC)
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
		public string Category { get; set; }

        /// <summary>
        /// Summary of event
        /// </summary>
		public string Summary { get; set; }

        /// <summary>
        /// Any additional data associated with the event
        /// </summary>
		public List<InternalProperty> Properties { get; set; }
    }
}
