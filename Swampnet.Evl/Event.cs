using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Swampnet.Evl
{
    public class Event
    {
        public Event()
        {
            TimestampUtc = DateTime.UtcNow;
        }

        public Guid Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Category Category { get; set; }

        public string Summary { get; set; }
        public DateTime TimestampUtc { get; set; }

        public EventProperty[] Properties { get; set; }
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
        Debug,
        Info,
        Error
    }
}
