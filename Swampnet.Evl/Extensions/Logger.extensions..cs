﻿using Serilog;
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

namespace Swampnet.Evl
{
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
                : base(formatProvider, null, null)
            {
                _dal = dal;
                _eventProcessor = eventProcessor;
            }


            protected override async Task PostAsync(IEnumerable<Event> events)
            {
                var ids = new List<Guid>();

                await Task.Delay(1); // Just to satisfy our async declaration for now.

                Parallel.ForEach(events, async evt =>
                {
                    try
                    {
                        var id = await _dal.CreateAsync(null, evt);

                        lock (ids)
                        {
                            ids.Add(id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });

                _eventProcessor.Enqueue(ids);
            }
        }
    }
}