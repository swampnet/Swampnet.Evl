using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using System.Collections.Concurrent;
using System.Threading;
using Swampnet.Evl.Contracts;
using Swampnet.Evl.Common.Entities;

namespace Swampnet.Evl.Services
{
    class EventQueueProcessor : IEventQueueProcessor
    {
        private readonly ConcurrentQueue<Guid> _queue = new ConcurrentQueue<Guid>();
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


        public void Enqueue(Guid id)
        {
            _queue.Enqueue(id);
            _queueEvent.Set();
        }


        public void Enqueue(IEnumerable<Guid> ids)
        {
            foreach(var id in ids.ToArray())
            {
                _queue.Enqueue(id);
            }
            _queueEvent.Set();
        }


        private IEnumerable<Guid> Wait()
        {
            _queueEvent.WaitOne();

            var ids = new List<Guid>();

            while(_queue.TryDequeue(out Guid result))
            {
                ids.Add(result);
            }

            return ids;
        }


        private async void MonitorThread()
        {
            var eventIds = Wait();
            while (eventIds != null)
            {
                foreach(var eventId in eventIds)
                {
                    try
                    {
                        var evt = await _eventDataAccess.ReadAsync(null, eventId);

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

                        await _eventDataAccess.UpdateAsync(null, eventId, evt);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, ex.Message);
                    }
                }

                eventIds = Wait();
            }
        }
    }
}
