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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("events"), StorageAccount("event-queue")] ICollector<string> queue,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            //log.LogInformation(requestBody);
            Test();

            // Create an ID if we don't already have one.
            var e = JsonConvert.DeserializeObject<Event>(requestBody);
            if (e.Id == Guid.Empty)
            {
                e.Id = Guid.NewGuid();
            }

            // At this point we just want to push it into a queue
            queue.Add(JsonConvert.SerializeObject(e));

            // We probably just want to return a 200 rather than echo the event back (We *could* return the Id or a small summary, but what do we do with
            // batch events?)
            return new OkObjectResult(e);
        }

        private static void Test()
        {
            //using(var context = new EventsContext())
            //{
            //    var xx = context.Events.ToArray();
            //}
        }
    }
}
