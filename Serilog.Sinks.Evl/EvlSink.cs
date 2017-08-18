using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Events;
using Serilog.Configuration;
using Swampnet.Evl.Common;
using System.Linq;

namespace Serilog.Sinks.Evl
{
    public class EvlSink : PeriodicBatchingSink
    {
        private static readonly int _defaultBatchSize = 10;                        // Maximum number of LogEvents in a batch
        private static readonly TimeSpan _defaultPeriod = TimeSpan.FromSeconds(5); // How often we flush the batch

        private readonly IFormatProvider _formatProvider;

        public EvlSink(IFormatProvider formatProvider)
            : this(_defaultBatchSize, _defaultPeriod)
        {
            _formatProvider = formatProvider;
        }


        protected EvlSink(int batchSizeLimit, TimeSpan period) 
            : base(batchSizeLimit, period)
        {
        }


        protected EvlSink(int batchSizeLimit, TimeSpan period, int queueLimit) 
            : base(batchSizeLimit, period, queueLimit)
        {
        }


        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            var evlEvents = Convert(events);
            Console.WriteLine($"EmitBatch: {evlEvents.Count()}");
            // @TODO: Post to Evl
            // @TODO: Need to take failure into account here. Serilog won't do anything to recover so we need to.
            //          - Can we add 'em back into the queue somehow?
        }


        // Convert Serilog LogEvent to an Evl.Event
        private IEnumerable<Event> Convert(IEnumerable<LogEvent> source)
        {
            var evlEvents = new List<Event>();

            foreach(var s in source)
            {
                var evlEvent = new Event();
                evlEvent.Summary = s.RenderMessage(_formatProvider);
                evlEvent.TimestampUtc = s.Timestamp.UtcDateTime;
                evlEvent.Category = s.Level.ToString();
                evlEvent.Properties = s.Properties.Select(p => new Property(p.Key, p.Value)).ToList();
                evlEvents.Add(evlEvent);
            }

            return evlEvents;
        }
    }
}
