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
        private readonly IEventDataAccess _dal;
        private readonly Organisation _evlOrganisation;

        public EventQueueProcessor(IAuth auth, IEventDataAccess dal, IEnumerable<IEventProcessor> processors)
        {
            _dal = dal;
            _processors = processors;

            _evlOrganisation = auth.GetEvlOrganisation();

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


        private void MonitorThread()
        {
            var eventIds = Wait();
            while (eventIds != null)
            {
                foreach(var eventId in eventIds)
                {
                    try
                    {
                        var evt = _dal.ReadAsync(_evlOrganisation, eventId).Result;

                        foreach (var processor in _processors.OrderBy(p => p.Priority))
                        {
                            try
                            {
                                processor.ProcessAsync(evt).Wait();
                            }
                            catch (Exception ex)
                            {
                                ex.AddData("Processor", processor.GetType().Name);
                                Log.Error(ex, ex.Message);
                            }
                        }

                        _dal.UpdateAsync(_evlOrganisation, eventId, evt).Wait();
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
