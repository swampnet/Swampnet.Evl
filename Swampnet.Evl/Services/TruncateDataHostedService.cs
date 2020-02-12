//using Microsoft.Extensions.Configuration;
//using Serilog;
//using Swampnet.Evl.Common.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Swampnet.Evl.Services
//{
//    class TruncateDataHostedService : BackgroundService
//    {
//        private readonly IEventDataAccess _eventDataAccess;
//        private readonly IConfiguration _cfg;
//        private DateTime _lastExecutedUtc = DateTime.MinValue;

//        public TruncateDataHostedService(IEventDataAccess eventDataAccess, IConfiguration cfg)
//        {
//            _eventDataAccess = eventDataAccess;
//            _cfg = cfg;
//        }


//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                try
//                {
//                    if (IsDue())
//                    {
//                        var sw = Stopwatch.StartNew();

//                        await _eventDataAccess.TruncateEventsAsync();

//                        _lastExecutedUtc = DateTime.UtcNow;

//                        Log.Debug("Truncate complete in {elapsed}s", sw.Elapsed.TotalSeconds.ToString("0.00"));
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Log.Error(ex, ex.Message);
//                }
//                finally
//                {
//                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
//                }
//            }
//        }


//        private bool IsDue()
//        {
//            TimeSpan ts;
//            if (!TimeSpan.TryParse(_cfg["evl:schedule:trunc-events"], out ts))
//            {
//                // Default to 1am
//                ts = TimeSpan.Parse("01:00");
//            }

//            return DateTime.UtcNow.Hour == ts.Hours 
//                && DateTime.UtcNow.Minute == ts.Minutes 
//                && _lastExecutedUtc < DateTime.UtcNow.AddHours(-23); // Last run was at least 23 hours ago
//        }
//    }
//}
