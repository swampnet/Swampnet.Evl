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
        private readonly IEnumerable<IEventProcessor> _processors;
        private readonly IAuth _auth;
        private readonly IEventDataAccess _eventDataAccess;

        public EventQueueProcessor(IAuth auth, IEventDataAccess dal, IEnumerable<IEventProcessor> processors)
        {
            _auth = auth;
            _eventDataAccess = dal;
            _processors = processors;

            _monitorThread = new Thread(MonitorThread)
            {
                IsBackground = true,
                Name = "EventProcessor Monitor"
            };

            _monitorThread.Start();
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


        private void MonitorThread()
        {
            var queuedEvents = Wait();

            while (queuedEvents != null)
            {
                var sw = Stopwatch.StartNew();

                // Can we flatten this & possibly group by organisation
                Parallel.ForEach(queuedEvents, async queuedEvent => {
                    foreach (var e in queuedEvent.Events)
                    {
                        try
                        {
                            var evt = await _eventDataAccess.CreateAsync(queuedEvent.Organisation, e);

                            foreach (var processor in _processors.OrderBy(p => p.Priority))
                            {
                                try
                                {
                                    await processor.ProcessAsync(evt);
                                }
                                catch (Exception ex)
                                {
                                    ex.AddData("Processor", processor.GetType().Name);
                                    Log.Error(ex, ex.Message);
                                }
                            }

                            await _eventDataAccess.UpdateAsync(null, evt);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, ex.Message);
                        }
                    }
                });

                Debug.WriteLine($">>> PROCESS in {sw.Elapsed.TotalMilliseconds}");

                queuedEvents = Wait();
            }
        }
    }
}
