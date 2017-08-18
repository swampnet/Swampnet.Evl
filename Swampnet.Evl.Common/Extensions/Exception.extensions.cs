using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public static class ExceptionExtensions
    {
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
