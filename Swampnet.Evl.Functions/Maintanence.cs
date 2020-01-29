using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Swampnet.Evl.Services.Interfaces;

namespace Swampnet.Evl.Functions
{
    public class Maintanence
    {
        private readonly IMaintanence _maintanence;

        public Maintanence(IMaintanence maintanence)
        {
            _maintanence = maintanence;
        }


        [FunctionName("maintanence")]
        public async Task Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            await _maintanence.RunAsync();
        }
    }
}
