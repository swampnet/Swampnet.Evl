using Serilog;
using Serilog.Sinks.Evl;
using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;

namespace Swampnet.Evl
{
    public static class LoggerExtensions
    {
        public static ILogger WithMembername(this ILogger logger, [CallerMemberName] string name = null)
        {
            return logger.ForContext("MemberName", name);
        }

        /// <summary>
        /// Add properties to log data
        /// </summary>
        /// <remarks>
        /// Usage:
        /// <code>
        /// Log.Logger.WithProperties(new[] {
        ///     new Property("one", "one-value"),
        ///     new Property("two", "two-value")
        /// }).Information("With a bunch of properties");
        /// </code>
        /// </remarks>
        /// <param name="logger"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static ILogger WithProperties(this ILogger logger, IEnumerable<IProperty> properties)
        {
            foreach (var p in properties)
            {
                logger = logger.ForContext(string.IsNullOrEmpty(p.Category) ? p.Name : p.Category + EvlSink.CATEGORY_SPLIT + p.Name, p.Value);
            }
            return logger;
        }


        public static ILogger WithPublicProperties(this ILogger logger, object o)
        {
            if(o == null)
            {
                return logger;
            }

            var properties = new List<Property>();
            foreach (PropertyInfo prop in o.GetType().GetProperties())
            {
                properties.Add(new Property(o.GetType().Name, prop.Name, prop.GetValue(o, null)));
            }

            return logger.WithProperties(properties);
        }


        public static ILogger WithKeyValuePairs(this ILogger logger, IEnumerable<KeyValuePair<string, object>> data)
        {
            foreach (var nv in data)
            {
                logger = logger.ForContext(nv.Key, nv.Value);
            }

            return logger;
        }


		public static ILogger WithTags(this ILogger logger, IEnumerable<string> tags)
		{
			return logger.WithProperties(tags.Select(t => new Property(EvlSink.TAG_CATEGORY, EvlSink.TAG_CATEGORY, t)));
		}
	}
}
