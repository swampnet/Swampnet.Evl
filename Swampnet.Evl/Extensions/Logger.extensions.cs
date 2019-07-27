using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Contracts;
using System.Diagnostics;
using System.Reflection;
using Swampnet.Evl.Common.Entities;
using Serilog.Sinks.Evl;

namespace Swampnet.Evl
{
    /// <summary>
    /// Logging / Serilog extensions
    /// </summary>
    internal static class LoggerExtensions
    {
        public static LoggerConfiguration LocalEvlSink(
                this LoggerSinkConfiguration loggerConfiguration,
                Guid orgId,
                IEventDataAccess dal, 
                IEventQueueProcessor eventProcessor,
				string source = null,
				string version = null,
                IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new LocalSink(orgId, dal, eventProcessor, source, version, formatProvider));
        }

       
        /// <summary>
        /// Local evl-serilog sink
        /// </summary>
        class LocalSink : EvlSink
        {
            private readonly IEventDataAccess _dal;
            private readonly IEventQueueProcessor _eventProcessor;
            private readonly Guid _orgId;

            public LocalSink(
                Guid orgId,
                IEventDataAccess dal,
                IEventQueueProcessor eventProcessor,
				string source,
				string version,
                IFormatProvider formatProvider)
                : base(formatProvider, null, null, source, version)
            {
                _orgId = orgId;
                _dal = dal;
                _eventProcessor = eventProcessor;
            }

            /// <summary>
            /// Not posting the events, creating them directly via the DAL (But still queing them up so we we process them as normal)
            /// </summary>
            /// <param name="events"></param>
            /// <returns></returns>
            protected override async Task PostAsync(IEnumerable<Event> events)
            {
                await Task.CompletedTask; // Just to satisfy our async declaration for now.

                Parallel.ForEach(events, evt =>
                {
                    try
                    {
                        // Check if we want to overrride the organisation
                        var orgOverride = evt.Properties.SingleOrDefault(p => p.Category.EqualsNoCase("__override__") && p.Name.EqualsNoCase("organisation-id"));
                        if(orgOverride != null)
                        {
                            evt.Properties.Remove(orgOverride);
                        }

						_eventProcessor.Enqueue(
                            orgOverride == null
                                ? _orgId
                                : Guid.Parse(orgOverride.Value), 
                            evt);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }
    }
}
