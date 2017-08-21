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
            if(properties != null && properties.Any())
            {
                var p = properties.SingleOrDefault(x => x.Name.EqualsNoCase(name));
                if (p != null)
                {
                    v = p.Value;
                }
            }

            return v;
		}


        public static int IntValue(this IEnumerable<IProperty> properties, string name, int defaultValue = 0)
        {
            int v = defaultValue;

            int.TryParse(properties.StringValue(name, defaultValue.ToString()), out v);

            return v;
        }


        public static double DoubleValue(this IEnumerable<IProperty> properties, string name, double defaultValue = 0.0)
        {
            double v = defaultValue;

            double.TryParse(properties.StringValue(name, defaultValue.ToString()), out v);

            return v;
        }


        public static DateTime DateTimeValue(this IEnumerable<IProperty> properties, string name)
        {
            return properties.DateTimeValue(name, DateTime.MinValue);
        }


        public static DateTime DateTimeValue(this IEnumerable<IProperty> properties, string name, DateTime defaultValue)
        {
            DateTime v = defaultValue;

            DateTime.TryParse(properties.StringValue(name, defaultValue.ToString()), out v);

            return v;
        }
    }
}
