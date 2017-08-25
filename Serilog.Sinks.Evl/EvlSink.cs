using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Events;
using Serilog.Configuration;
using Swampnet.Evl.Client;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl;
using Serilog.Debugging;

namespace Serilog.Sinks.Evl
{
    public class EvlSink : PeriodicBatchingSink
    {
        private static readonly int _defaultBatchSize = 50;                        // Maximum number of LogEvents in a batch
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



        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var evlEvents = Convert(events);

            try
            {
                await Api.PostAsync(_failedEvents.Concat(evlEvents));

                _failedEvents.Clear();
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("Unable to write {0} log events due to following error: {1}", events.Count(), ex.Message);

                // @TODO: Should probably do something here to stop this growing out of control...
                _failedEvents.AddRange(evlEvents);

                throw;
            }
        }

        private readonly List<Event> _failedEvents = new List<Event>();

        // Convert Serilog LogEvent to an Evl.Event
        private IEnumerable<Event> Convert(IEnumerable<LogEvent> source)
        {
            var evlEvents = new List<Event>();

            foreach(var s in source)
            {
                var evlEvent = new Event();
				evlEvent.Source = "@TODO: SOURCE"; // Although, we infer that from the api-key don't we?
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
