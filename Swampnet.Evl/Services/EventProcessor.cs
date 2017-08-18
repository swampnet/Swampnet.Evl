using Swampnet.Evl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swampnet.Evl.Common;
using Serilog;
using System.Collections.Concurrent;
using System.Threading;

namespace Swampnet.Evl.Services
{
    class EventProcessorQueue : IEventProcessorQueue
    {
        private readonly ConcurrentQueue<Event> _queue = new ConcurrentQueue<Event>();
        private readonly AutoResetEvent _queueEvent = new AutoResetEvent(false);
        private readonly Thread _monitorThread;
        private readonly IEnumerable<IEventProcessor> _processors;

        public EventProcessorQueue(IEnumerable<IEventProcessor> processors)
        {
            _processors = processors;
            _monitorThread = new Thread(MonitorThread)
            {
                IsBackground = true,
                Name = "EventProcessor Monitor"
            };

            _monitorThread.Start();
        }


        public void Enqueue(Event evt)
        {
            _queue.Enqueue(evt);
            _queueEvent.Set();
        }


        public void Enqueue(IEnumerable<Event> evts)
        {
            foreach(var evt in evts)
            {
                Enqueue(evt);
            }
        }


        private IEnumerable<Event> Wait()
        {
            _queueEvent.WaitOne();

            var evts = new List<Event>();

            while(_queue.TryDequeue(out Event result))
            {
                evts.Add(result);
            }

            return evts;
        }


        private void MonitorThread()
        {
            var evts = Wait();
            while (evts != null)
            {
                Log.Debug("Process {EventCount} events", evts.Count());
                foreach(var evt in evts.OrderBy(e => e.TimestampUtc))
                {
                    try
                    {
                        foreach(var processor in _processors.OrderBy(p => p.Priority))
                        {
                            try
                            {
                                processor.Process(evt);
                            }
                            catch (Exception ex)
                            {
                                ex.AddData("Processor", processor.GetType().Name);
                                Log.Error(ex, ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, ex.Message);
                    }
                }

                evts = Wait();
            }
        }
    }
}
