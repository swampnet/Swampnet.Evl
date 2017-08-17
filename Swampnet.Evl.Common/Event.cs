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
	}


	public class Property : IProperty
	{
		public string Category { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }
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
