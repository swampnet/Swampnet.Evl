using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Swampnet.Evl
{
    public class Event
    {
        public Event()
        {
            TimestampUtc = DateTime.UtcNow;
            History = new List<EventHistory>();
            Tags = new List<string>();
        }

        public Guid Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Category Category { get; set; }
        public string Source { get; set; }

        public string Summary { get; set; }
        public DateTime TimestampUtc { get; set; }
        public List<string> Tags { get; set; }

        public Property[] Properties { get; set; }
        public List<EventHistory> History { get; set; }
    }


    public class EventSummary
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Category Category { get; set; }
        public string Source { get; set; }
        public string Summary { get; set; }
        public DateTime TimestampUtc { get; set; }
        public List<string> Tags { get; set; }
    }


    public class EventHistory
    {
        public EventHistory()
        {
            TimestampUtc = DateTime.UtcNow;
        }

        public EventHistory(string type, string details = null)
            : this()
        {
            Type = type;
            Details = details;
        }

        public DateTime TimestampUtc { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
    }


    public enum Category
    {
        debug,
        info,
        error
    }
}
