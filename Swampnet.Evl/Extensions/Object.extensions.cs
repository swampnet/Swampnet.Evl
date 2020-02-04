using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Swampnet.Evl
{
    public static class ObjectExtensions
    {
        public static string SerializeXml(this object o)
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
    }
}
