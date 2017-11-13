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
        /// <summary>
        /// Add current method name to the log
        /// </summary>
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
                logger = logger.ForContext(GetName(p), p.Value);
            }
            return logger;
        }


        /// <summary>
        /// Add single property to log
        /// </summary>
        public static ILogger WithProperty(this ILogger logger, IProperty property)
        {
            return logger.WithProperties(new[] { property });
        }


        /// <summary>
        /// Add single property to log
        /// </summary>
        public static ILogger WithProperty(this ILogger logger, string name, object value, string category = null)
        {
            return logger.WithProperty(new Property(category, name, value));
        }


        /// <summary>
        /// Add key/value pairs to the log
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ILogger WithKeyValuePairs(this ILogger logger, IEnumerable<KeyValuePair<string, object>> data)
        {
            foreach (var nv in data)
            {
                logger = logger.ForContext(nv.Key, nv.Value);
            }

            return logger;
        }


        /// <summary>
        /// Add a tag to the log
        /// </summary>
        public static ILogger WithTag(this ILogger logger, string tag)
        {
            return logger.WithTags(new[] { tag });
        }


        /// <summary>
        /// Add multiple tags to the log
        /// </summary>
        public static ILogger WithTags(this ILogger logger, IEnumerable<string> tags)
		{
			return logger.WithProperties(tags.Select(t => new Property(EvlSink.TAG_CATEGORY, EvlSink.TAG_CATEGORY, t)));
		}


        private static string GetName(IProperty p)
        {
            // I've already forgotton what this godawful id thing is all about. Something about making property names unique (I think Serilog stores all
            // this in a dictionary, so we can't have multiple properties with the same name. Evl, of course, doesn't care about that (and in fact we
            // use multiple properties with the same name all over the place)
            // Pretty sure this is some kind of way to trick the Serilog subsystem to handle that.
            return (_id++).ToString() + EvlSink.ID + (string.IsNullOrEmpty(p.Category) ? p.Name : p.Category + EvlSink.CATEGORY_SPLIT + p.Name);
        }

        private static long _id = 0;
    }
}
