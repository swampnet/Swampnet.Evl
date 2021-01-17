using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Swampnet.Evl
{
    public class EventSearchCriteria
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;

        public Guid? Id { get; set; }
        public string Summary { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Source { get; set; }
        public string Tags { get; set; }
        public bool ShowDebug { get; set; }
        public bool ShowInformation { get; set; } = true;
        public bool ShowError { get; set; } = true;
    }


    public class EventSearchResult
    {
        public EventSummary[] Events { get; set; }
        public int TotalCount { get; set; }
        public TimeSpan Elapsed { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }


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
