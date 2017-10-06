using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        [JsonConverter(typeof(StringEnumConverter))]
        public EventCategory Category { get; set; }

        /// <summary>
        /// Summary of event
        /// </summary>
		public string Summary { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        public string Source { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {TimestampUtc:s} [{Source}] [{Category}] {Summary}";
        }
    }
}
