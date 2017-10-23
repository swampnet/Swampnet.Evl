using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Evl;
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

namespace Swampnet.Evl
{
    /// <summary>
    /// Logging / Serilog extensions
    /// </summary>
    public static class LoggerExtensions
    {
        public static LoggerConfiguration LocalEvlSink(
                this LoggerSinkConfiguration loggerConfiguration,
                IEventDataAccess dal, 
                IEventQueueProcessor eventProcessor,
                IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new LocalSink(dal, eventProcessor, formatProvider));
        }

       
        /// <summary>
        /// Local evl-serilog sink
        /// </summary>
        class LocalSink : EvlSink
        {
            private readonly IEventDataAccess _dal;
            private readonly IEventQueueProcessor _eventProcessor;

            public LocalSink(
                IEventDataAccess dal,
                IEventQueueProcessor eventProcessor,
                IFormatProvider formatProvider)
                : base(formatProvider, null, null, null, null)
            {
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
                await Task.Delay(1); // Just to satisfy our async declaration for now.

                Parallel.ForEach(events, async evt =>
                {
                    try
                    {
                        var id = await _dal.CreateAsync(null, Common.Convert.ToEventDetails(evt));

						_eventProcessor.Enqueue(id);
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
