using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Swampnet.Evl
{
    public static class StringExtensions
    {
        /// <summary>
        /// Perform a case insensitive comparison
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool EqualsNoCase(this string lhs, string rhs)
        {
            // Both null -> true
            if (lhs == null && rhs == null)
            {
                return true;
            }

            // One null -> false
            if (lhs == null || rhs == null)
            {
                return false;
            }

            return lhs.Equals(rhs, StringComparison.OrdinalIgnoreCase);
        }

        public static T DeserializeXml<T>(this string source)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(source))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

    }
}
