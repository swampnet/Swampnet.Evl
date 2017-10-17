using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Swampnet.Evl
{
    public static class StringExtensions
    {
        public static T Deserialize<T>(this string xml)
        {
            var rtn = default(T);

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xml))
            {
                rtn = (T)serializer.Deserialize(reader);
            }

            return rtn;
        }


        /// <summary>
        /// Truncate a string
        /// </summary>
        public static string Truncate(this string value, int maxLength, bool withEllipses = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : (value.Substring(0, maxLength) + (withEllipses ? "..." : ""));
        }
    }
}
