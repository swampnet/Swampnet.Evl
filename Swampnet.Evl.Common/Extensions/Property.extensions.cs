using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Swampnet.Evl
{
    /// <summary>
    /// IProperty extensions / helper methods
    /// </summary>
	public static class PropertyExtensions
	{
        /// <summary>
        /// Return the value of a property as a string value
        /// </summary>
        /// <remarks>
        /// Performs a case-insensitive search and returns the value of the specified property as a string.
        /// </remarks>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Return the value of a property as an integer value
        /// </summary>
        /// <remarks>
        /// Performs a case-insensitive search and returns the value of the specified property as an integer.
        /// </remarks>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int IntValue(this IEnumerable<IProperty> properties, string name, int defaultValue = 0)
        {
            int v = defaultValue;

            int.TryParse(properties.StringValue(name, defaultValue.ToString()), out v);

            return v;
        }


        /// <summary>
        /// Return the value of a property as a double value
        /// </summary>
        /// <remarks>
        /// Performs a case-insensitive search and returns the value of the specified property as a double.
        /// </remarks>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double DoubleValue(this IEnumerable<IProperty> properties, string name, double defaultValue = 0.0)
        {
            double v = defaultValue;

            double.TryParse(properties.StringValue(name, defaultValue.ToString()), out v);

            return v;
        }


        /// <summary>
        /// Return the value of a property as a DateTime value
        /// </summary>
        /// <remarks>
        /// Performs a case-insensitive search and returns the value of the specified property as a DateTime.
        /// </remarks>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DateTime DateTimeValue(this IEnumerable<IProperty> properties, string name)
        {
            return properties.DateTimeValue(name, DateTime.MinValue);
        }


        /// <summary>
        /// Return the value of a property as a DateTime value
        /// </summary>
        /// <remarks>
        /// Performs a case-insensitive search and returns the value of the specified property as a DateTime.
        /// </remarks>
        /// <param name="properties"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DateTime DateTimeValue(this IEnumerable<IProperty> properties, string name, DateTime defaultValue)
        {
            DateTime v = defaultValue;

            DateTime.TryParse(properties.StringValue(name, defaultValue.ToString()), out v);

            return v;
        }
    }
}
