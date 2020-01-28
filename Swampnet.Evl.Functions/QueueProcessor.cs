using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Swampnet.Evl.Functions
{
    public static class QueueProcessor
    {
        [FunctionName("queue-processor")]
        public static async Task Run([QueueTrigger("events", Connection = "event-queue")]string json, ILogger log)
        {
            var e = JsonConvert.DeserializeObject<Event>(json);

            //throw new ApplicationException("test");
            log.LogInformation($"de-queued event: {e.Id} / {e.Summary}");
            await Task.Delay(2000);
            log.LogInformation($"complete: {e.Id} / {e.Summary}");
        }
    }
}
