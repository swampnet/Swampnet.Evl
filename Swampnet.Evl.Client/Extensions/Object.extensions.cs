using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Swampnet.Evl
{
    public static class ObjectExtensions
    {
        public static IEnumerable<IProperty> GetPublicProperties(this object o)
        {
            if(o == null)
            {
                return Enumerable.Empty<IProperty>();
            }

            var properties = new List<Property>();
            foreach (PropertyInfo prop in o.GetType().GetProperties())
            {
                properties.Add(new Property(o.GetType().Name, prop.Name, prop.GetValue(o, null)));
            }

            return properties;
        }
    }
}
