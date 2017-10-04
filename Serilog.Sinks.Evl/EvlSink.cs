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
using System.Diagnostics;
using System.Reflection;

namespace Serilog.Sinks.Evl
{
    public class EvlSink : PeriodicBatchingSink
    {
		public const string CATEGORY_SPLIT = "@@@";

        private static readonly int _defaultBatchSize = 50;                        // Maximum number of LogEvents in a batch
        private static readonly TimeSpan _defaultPeriod = TimeSpan.FromSeconds(5); // How often we flush the batch

        private readonly IFormatProvider _formatProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="apiKey"></param>
        /// <param name="endpoint"></param>
        /// <param name="source">Event source. Defaults to executing assembly name</param>
        /// <param name="sourceVersion">Event source version. Defaults to executing assembly version</param>
        public EvlSink(IFormatProvider formatProvider, string apiKey, string endpoint, string source, string sourceVersion)
            : this(_defaultBatchSize, _defaultPeriod)
        {
            Api.ApiKey = apiKey;
            Api.Endpoint = endpoint;

            if (string.IsNullOrEmpty(source))
            {
                var name = Assembly.GetEntryAssembly().GetName();
                source = name.Name;
                sourceVersion = name.Version.ToString();
            }

            Api.Source = source;
            Api.SourceVersion = sourceVersion;

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
                await PostAsync(_failedEvents.Concat(evlEvents));

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


        protected virtual Task PostAsync(IEnumerable<Event> events)
        {
            return Api.PostAsync(events);
        }


        private readonly List<Event> _failedEvents = new List<Event>();


        // Convert Serilog LogEvent to an Evl.Event
        protected IEnumerable<Event> Convert(IEnumerable<LogEvent> source)
        {
            var evlEvents = new List<Event>();

            foreach(var s in source)
            {
                var evlEvent = new Event();
                evlEvent.Source = Api.Source;
                evlEvent.SourceVersion = Api.SourceVersion;
                evlEvent.Summary = s.RenderMessage(_formatProvider);
                evlEvent.TimestampUtc = s.Timestamp.UtcDateTime;
                evlEvent.Category = Convert(s.Level);

                evlEvent.Properties = new List<Property>();
                Process(evlEvent.Properties, s.Properties);

                evlEvents.Add(evlEvent);
            }

            return evlEvents;
        }

        private EventCategory Convert(LogEventLevel level)
        {
            EventCategory cat = EventCategory.Information;

            switch (level)
            {
                case LogEventLevel.Information:
                    cat = EventCategory.Information;
                    break;

                case LogEventLevel.Fatal:
                case LogEventLevel.Error:
                    cat = EventCategory.Error;
                    break;

                case LogEventLevel.Debug:
                    cat = EventCategory.Debug;
                    break;

                case LogEventLevel.Warning:
                    cat = EventCategory.Warning;
                    break;

                default:
                    throw new NotSupportedException($"Serilog event category {level} not supported");
            }

            return cat;
        }

        private void Process(List<Property> properties, IReadOnlyDictionary<string, LogEventPropertyValue> logEventValues)
        {
            foreach(var logEventValue in logEventValues)
            {
                var scalar = logEventValue.Value as ScalarValue;
                if(scalar != null)
                {
					var parts = Split(logEventValue.Key);

					properties.Add(new Property(parts.Item1, parts.Item2, scalar.Value));
				}

                var d = logEventValue.Value as DictionaryValue;
                if(d != null)
                {
                    Process(properties, null, d.Elements);
                }

                var seq = logEventValue.Value as SequenceValue;
                // @TODO: Handle SequenceValue's

                var str = logEventValue.Value as StructureValue;
                // @TODO: Handle StructureValue's
            }
        }


		private Tuple<string, string> Split(string key)
		{
			string category = "";
			string name = "key";

			if (key.Contains(EvlSink.CATEGORY_SPLIT))
			{
				category = key.Substring(0, key.IndexOf(EvlSink.CATEGORY_SPLIT));
				name = key.Substring(key.IndexOf(EvlSink.CATEGORY_SPLIT) + EvlSink.CATEGORY_SPLIT.Length);
			}

			return new Tuple<string, string>(category, name);
		}


        private void Process(List<Property> properties, string category, IReadOnlyDictionary<ScalarValue, LogEventPropertyValue> logEventValues)
        {
            foreach (var logEventValue in logEventValues)
            {
                var scalar = logEventValue.Value as ScalarValue;
                if (scalar != null)
                {
                    properties.Add(new Property(category, logEventValue.Key.ToString(), scalar.Value));
                }

                var d = logEventValue.Value as DictionaryValue;
                if (d != null)
                {
                    Process(properties, logEventValue.Key.ToString(), d.Elements);
                }

                var seq = logEventValue.Value as SequenceValue;
                // @TODO: Handle SequenceValue's

                var str = logEventValue.Value as StructureValue;
                // @TODO: Handle StructureValue's
            }
        }
    }
}
