using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Swampnet.Evl
{
	public static class PropertyExtensions
	{
		public static void Add(this ICollection<IProperty> properties, IProperty property)
		{
			properties?.Add(property);
		}


		public static string StringValue(this IEnumerable<IProperty> properties, string name, string defaultValue = "")
		{
			string v = defaultValue;

			var p = properties.SingleOrDefault(x => x.Name.EqualsNoCase(name));
			if(p != null)
			{
				v = p.Value;
			}

			return v;
		}
	}
}
