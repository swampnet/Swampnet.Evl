using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Swampnet.Evl.Client
{
    public enum EventCategory
    {
        Debug,
        Information,
        Warning,
        Error
    }

    /// <summary>
    /// An event
    /// </summary>
	public class Event
	{
        /// <summary>
        /// Event Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Event timestamp (UTC)
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// Last time event was updated
        /// </summary>
        /// <remarks>
        /// We sometimes manipulate an event after it's been captured (We might add properties or change its category
        /// due to a rule firing)
        /// This value tracks when we last changed it
        /// </remarks>
        [JsonIgnore]
        public DateTime? LastUpdatedUtc { get; set; }

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
        /// Any additional data associated with the event
        /// </summary>
		public List<Property> Properties { get; set; }

        /// <summary>
        /// Event source
        /// </summary>
        public string Source { get; set; }

        public string SourceVersion { get; set; }
        
        /// <summary>
        /// Event tags
        /// </summary>
        public string[] Tags { get; set; }

        public override string ToString()
        {
            return $"{TimestampUtc:s} [{Source}] [{Category}] {Summary}";
        }
    }


	public class Property : IProperty
	{
        public Property()
        {
        }


        public Property(string name, object value)
            : this("", name, value)
        {
        }

        public Property(string category, string name, object value)
        {
            Category = category;
            Name = name;
            Value = value?.ToString();
        }

        public string Category { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Category) 
                ? $"{Name} = {Value}" 
                : $"[{Category}] {Name} = {Value}";
        }
    }
}
