using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using System.Collections.Concurrent;
using System.Threading;
using Swampnet.Evl.Contracts;
using Swampnet.Evl.Common.Entities;
using System.Threading.Tasks;
using Swampnet.Evl.Client;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.Services
{
    class EventQueueProcessor : IEventQueueProcessor
    {
        class QueuedEvents
        {
            public QueuedEvents(Guid org, Client.Event evt)
                : this(org, new[] { evt })
            {
            }

            public QueuedEvents(Guid org, IEnumerable<Client.Event> evts)
            {
                Organisation = org;
                Events = evts;
            }


            public Guid Organisation { get; private set; }
            public IEnumerable<Client.Event> Events { get; private set; }
        }


        private readonly ConcurrentQueue<QueuedEvents> _queue = new ConcurrentQueue<QueuedEvents>();
        private readonly AutoResetEvent _queueEvent = new AutoResetEvent(false);
        private readonly Thread _monitorThread;
        private readonly IConfiguration _configuration;

        public EventQueueProcessor(IConfiguration configuration)
        {
            _monitorThread = new Thread(MonitorThread)
            {
                IsBackground = true,
                Name = "EventProcessor Monitor"
            };

            _monitorThread.Start();
            _configuration = configuration;
        }


        public void Enqueue(Guid org, Client.Event evt)
        {
            _queue.Enqueue(new QueuedEvents(org, evt));
            _queueEvent.Set();
        }

        public void Enqueue(Guid org, IEnumerable<Client.Event> evts)
        {
            _queue.Enqueue(new QueuedEvents(org, evts));
            _queueEvent.Set();
        }


        private IEnumerable<QueuedEvents> Wait()
        {
            _queueEvent.WaitOne();

            var evts = new List<QueuedEvents>();

            while(_queue.TryDequeue(out QueuedEvents evt))
            {
                evts.Add(evt);
            }

            return evts;
        }


        private async void MonitorThread()
        {
            var queuedEvents = Wait();

            while (queuedEvents != null)
            {
                var sw = Stopwatch.StartNew();

                // Just pushing to v2
                await queuedEvents
                    .SelectMany(x => x.Events)
                    .Select(s => v2.Event.FromLegacyEvent(s))
                    .PostAsync(_configuration.GetValue<string>("evl2:api-key"));

                Debug.WriteLine($">>> PROCESS in {sw.Elapsed.TotalMilliseconds}");

                queuedEvents = Wait();
            }
        }
    }
}
