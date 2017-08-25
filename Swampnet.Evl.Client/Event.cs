using System;
using System.Collections.Generic;

namespace Swampnet.Evl.Client
{
    /// <summary>
    /// An event
    /// </summary>
	public class Event
	{
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

        /// <summary>
        /// Any additional data associated with the event
        /// </summary>
		public List<Property> Properties { get; set; }

        /// <summary>
        /// Event source
        /// </summary>
        /// <remarks>
        /// @TODO: Not sure about this, the source should probably be infered from whatever api-key was used to
        ///        generate the event.
        /// </remarks>
        public string Source { get; set; }

		public override string ToString()
        {
            return $"{TimestampUtc:s} [{Category}] {Summary}";
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


	///// <summary>
	///// Event summary - Push this down to client for 'master' view.
	///// </summary>
	//public class EventSummary
	//{
	//	public long Id { get; set; }
	//	public DateTime TimestampUtc { get; set; }
	//	public string Category { get; set; }
	//	public string Summary { get; set; }
	//	public string ApplicationName { get; set; }
	//}
}
