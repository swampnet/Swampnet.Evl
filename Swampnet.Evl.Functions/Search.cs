using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace Swampnet.Evl.Functions
{
    public static class Search
    {
        [FunctionName("search")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            await Task.CompletedTask;

            return new OkObjectResult(Enumerable.Range(0, 100).Select(x => new Event()
            {
                Id = Guid.NewGuid(),
                Summary = $"Event {x}",
                TimestampUtc = DateTime.Now.AddSeconds(-x)
            }));
        }
    }
}
