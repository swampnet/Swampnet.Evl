using System;
using System.Collections.Generic;

namespace Swampnet.Evl.Common
{
	public class Event
	{
		/// <summary>
		/// Time event was raised (UTC)
		/// </summary>
		public DateTime TimestampUtc { get; set; }

		public string Category { get; set; }

		public string Summary { get; set; }

		public List<Property> Properties { get; set; }

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
