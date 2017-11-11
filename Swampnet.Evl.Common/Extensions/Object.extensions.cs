using Swampnet.Evl.Client;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Swampnet.Evl
{
    public static class ObjectExtensions
    {
        public static string ToXmlString(this object o)
        {
            try
            {
                string xml = o as string;

                if (xml == null)
                {
                    if (o != null)
                    {
                        var s = new XmlSerializer(o.GetType());

                        using (var sw = new StringWriter())
                        {
                            s.Serialize(sw, o);
                            xml = sw.ToString();
                        }
                    }
                    else
                    {
                        xml = "<null/>";
                    }
                }

                return xml;
            }
            catch (Exception ex)
            {
                ex.AddData("o", o == null ? "null" : o.GetType().Name);
                throw;
            }
        }

        public static IEnumerable<IProperty> GetPublicProperties(this object o)
        {
            if (o == null)
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
