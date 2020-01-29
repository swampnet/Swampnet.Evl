using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swampnet.Evl.Services.Interfaces;

namespace Swampnet.Evl.Functions
{
    public class QueueProcessor
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IProcess _process;

        public QueueProcessor(IEventsRepository eventsRepository, IProcess process)
        {
            _eventsRepository = eventsRepository;
            _process = process;
        }


        [FunctionName("queue-processor")]
        public async Task Run([QueueTrigger("events", Connection = "event-queue")]string json, ILogger log)
        {
            var e = JsonConvert.DeserializeObject<Event>(json);

            log.LogInformation($"de-queued event: {e.Id} / {e.Summary}");

            // Save event to DB
            await _eventsRepository.SaveAsync(e);

            log.LogInformation($"saved: {e.Id}");

            // Run any processors we have registered
            await _process.ProcessEventAsync(e.Id);

            log.LogInformation($"complete: {e.Id}");
        }
    }
}
