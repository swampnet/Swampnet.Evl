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

        public QueueProcessor(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }


        [FunctionName("queue-processor")]
        public async Task Run([QueueTrigger("events", Connection = "event-queue")]string json, ILogger log)
        {
            var e = JsonConvert.DeserializeObject<Event>(json);

            log.LogInformation($"de-queued event: {e.Id} / {e.Summary}");

            await _eventsRepository.SaveAsync(e);

            log.LogInformation($"complete: {e.Id} / {e.Summary}");
        }
    }
}
