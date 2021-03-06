﻿using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Common.Contracts;
//using Swampnet.Evl.EventProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl
{
    static class ServiceCollectionExtensions
    {
        public static void AddDefaultActionHandlers(this IServiceCollection services)
        {
            //services.AddTransient<IActionHandler, AddPropertyActionHandler>();
            //services.AddTransient<IActionHandler, AddTagActionHandler>();
            //services.AddTransient<IActionHandler, RemoveTagActionHandler>();
            //services.AddTransient<IActionHandler, ChangeCategoryActionHandler>();
            //services.AddTransient<IActionHandler, DebugActionHandler>();            
        }


        public static void AddDefaultEventProcessors(this IServiceCollection services)
        {
            //services.AddSingleton<IEventProcessor, RuleEventProcessor>();
            //services.AddSingleton<IEventProcessor, DummyEventProcessor>();
        }
    }
}
