using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swampnet.Evl.Services.Interfaces;

namespace Swampnet.Evl.Functions
{
    public class Processor
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IRuleProcessor _process;

        public Processor(IEventsRepository eventsRepository, IRuleProcessor process)
        {
            _eventsRepository = eventsRepository;
            _process = process;
        }


        [FunctionName("queue-processor")]
        public async Task Run([QueueTrigger("events", Connection = "event-queue")]string json, ILogger log)
        {
            var e = JsonConvert.DeserializeObject<Event>(json);

            log.LogInformation($"de-queued event: {e.Id} / {e.Summary}");
            
            e.History.Add(new EventHistory("dequeued"));

            if (e.Summary.Length > 512)
            {
                e.History.Add(new EventHistory()
                {
                    Type = "truncate",
                    Details = $"Truncating summary (length: {e.Summary.Length})"
                });
                e.Summary = e.Summary.Substring(0, 500) + " ...";
            }

            // Save event to DB
            await _eventsRepository.SaveAsync(e);

            log.LogInformation($"saved: {e.Id}");

            // Run any processors we have registered. Should this be another queue?
            await _process.ProcessEventAsync(e.Id);

            log.LogInformation($"complete: {e.Id}");
        }
    }
}
