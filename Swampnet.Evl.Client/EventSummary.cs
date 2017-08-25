using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Client
{
    public class EventSummary
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Event timestamp (UTC)
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
        /// <remarks>
        /// eg, Information, Error, Warning etc
        /// </remarks>
		public string Category { get; set; }

        /// <summary>
        /// Summary of event
        /// </summary>
		public string Summary { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {TimestampUtc:s} [{Category}] {Summary}";
        }
    }
}
