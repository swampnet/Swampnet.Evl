using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
