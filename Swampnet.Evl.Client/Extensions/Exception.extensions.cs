using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;

namespace Swampnet.Evl
{
    /// <summary>
    /// Exception extensions / helper methods
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Add / Update exception Data value
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddData(this Exception ex, string key, object value)
        {
            if (ex.Data.Contains(key))
            {
                ex.Data[key] = value;
            }
            else
            {
                ex.Data.Add(key, value);
            }
        }


        public static void AddData(this Exception ex, IProperty property)
        {
            if (ex.Data.Contains(property.Name))
            {
                ex.Data[property.Name] = property.Value;
            }
            else
            {
                ex.Data.Add(property.Name, property.Value);
            }
        }


        public static void AddData(this Exception ex, IEnumerable<IProperty> properties)
        {
            foreach(var p in properties)
            {
                ex.AddData(p);
            }
        }
    }
}
