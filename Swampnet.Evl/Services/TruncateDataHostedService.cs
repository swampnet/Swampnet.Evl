using Microsoft.Extensions.Configuration;
using Serilog;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Swampnet.Evl.Services
{
    class TruncateDataHostedService : BackgroundService
    {
        private readonly IEventDataAccess _eventDataAccess;
        private readonly IConfiguration _cfg;

        public TruncateDataHostedService(IEventDataAccess eventDataAccess, IConfiguration cfg)
        {
            _eventDataAccess = eventDataAccess;
            _cfg = cfg;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    await _eventDataAccess.TruncateEventsAsync();

                    Log.Debug("Truncate complete in {elapsed}s", sw.Elapsed.TotalSeconds.ToString("0.00"));
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
                finally
                {
                    // This doesn't need to run every hour - At most it's once a day...
                    await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
                }
            }
        }
    }
}
