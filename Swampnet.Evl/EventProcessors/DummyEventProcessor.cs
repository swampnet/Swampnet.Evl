﻿//using Swampnet.Evl.Common.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Swampnet.Evl.Client;
//using Serilog;
//using Swampnet.Evl.Common.Entities;

//namespace Swampnet.Evl.EventProcessors
//{
//    class DummyEventProcessor : IEventProcessor
//    {
//        public int Priority => 0;

//        public Task ProcessAsync(EventDetails evt)
//        {
//            //Log.Information("Processing evt {EventSummary} {Processor}", evt.Summary, this.GetType().Name);
//            return Task.CompletedTask;
//        }
//    }

//    class AnotherDummyEventProcessor : IEventProcessor
//    {
//        public int Priority => 0;

//        public Task ProcessAsync(EventDetails evt)
//        {
//            //Log.Information("Processing evt {EventSummary} {Processor}", evt.Summary, this.GetType().Name);
//            return Task.CompletedTask;
//        }
//    }
//}
