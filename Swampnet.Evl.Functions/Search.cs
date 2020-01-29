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
using Swampnet.Evl.Services.Interfaces;

namespace Swampnet.Evl.Functions
{
    public class Search
    {
        private readonly IEventsRepository _eventsRepository;

        public Search(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }


        [FunctionName("search")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"get",Route = null)] HttpRequest req, 
            ILogger log)
        {
            log.LogInformation("Running ITest.Boosh()");

            var events = await _eventsRepository.SearchAsync();

            return new OkObjectResult(events);
        }
    }
}
