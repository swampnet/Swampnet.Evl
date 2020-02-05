using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Swampnet.Evl.Functions
{
    public static class Post
    {
        [FunctionName("post-event")]
        public static async Task<IActionResult> PostEvent(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("events"), StorageAccount("event-queue")] ICollector<string> queue,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var evt = JsonConvert.DeserializeObject<Event>(requestBody);
            log.LogInformation($"Enqueuing 1 event");
            Enqueue(queue, evt);

            return new OkResult();
        }


        [FunctionName("post-bulk")]
        public static async Task<IActionResult> PostBulk(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("events"), StorageAccount("event-queue")] ICollector<string> queue,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var events = JsonConvert.DeserializeObject<Event[]>(requestBody);
            log.LogInformation($"Enqueuing {events.Count()} events");
            foreach(var e in events)
            {
                Enqueue(queue, e);
            }

            return new OkResult();
        }


        private static void Enqueue(ICollector<string> queue, Event e)
        {
            // Create an ID if we don't already have one.
            if (e.Id == Guid.Empty)
            {
                e.Id = Guid.NewGuid();
            }

            e.History.Add(new EventHistory("enqueued"));

            // At this point we just want to push it into a queue
            queue.Add(JsonConvert.SerializeObject(e));
        }
    }
}
