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
        private readonly ITest _test;

        public Search(ITest test)
        {
            _test = test;
        }


        [FunctionName("search")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"get",Route = null)] HttpRequest req, 
            ILogger log)
        {
            await Task.CompletedTask;

            log.LogInformation("Running ITest.Boosh()");

            var events = _test.Boosh();

            return new OkObjectResult(events);
        }
    }
}
