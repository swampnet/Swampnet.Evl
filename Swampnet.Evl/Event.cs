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
        }

        public Guid Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Category Category { get; set; }
        public string Source { get; set; }

        public string Summary { get; set; }
        public DateTime TimestampUtc { get; set; }

        public EventProperty[] Properties { get; set; }
        public List<EventHistory> History { get; set; }
    }


    public class EventHistory
    {
        public EventHistory()
        {
            TimestampUtc = DateTime.UtcNow;
        }

        public EventHistory(EventHistoryType type, string details = null)
            : this()
        {
            Type = type;
            Details = details;
        }

        public DateTime TimestampUtc { get; set; }
        public EventHistoryType Type { get; set; }
        public string Details { get; set; }
    }


    public class EventProperty
    {
        public EventProperty()
        {
        }

        public EventProperty(string name, object value, string category = "")
        {
            Name = name;
            Value = value?.ToString();
        }

        public string Category { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }


    public enum Category
    {
        debug,
        info,
        error
    }


    public enum EventHistoryType
    {
        Queued,
        Processed,
        Complete,
        Error
    }
}
