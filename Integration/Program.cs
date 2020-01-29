﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Services;
using Swampnet.Evl.Services.Interfaces;
using System.Threading.Tasks;

namespace Integration
{
    interface IProgram 
    {
        Task Run();
    }

    class Program : IProgram
    {
        //private static IConfiguration _config;
        private readonly ITest _test;
        private readonly IEventsRepository _eventsRepository;
        private readonly IProcess _process;

        static async Task Main(string[] args)
        {
            //setup DI
            var serviceProvider = new ServiceCollection()
                .RegisterServiceTypes()
                .AddSingleton<IProgram, Program>()
                .BuildServiceProvider();

            await serviceProvider.GetService<IProgram>().Run();
        }


        public Program(ITest test, IEventsRepository eventsRepository, IProcess process)
        {
            _test = test;
            _eventsRepository = eventsRepository;
            _process = process;
        }

        
        public async Task Run()
        {
            //var evt = new Swampnet.Evl.Event()
            //{
            //    Category = Swampnet.Evl.Category.info,
            //    Source = "test-03",
            //    Summary = $"Test @ {DateTime.UtcNow}",
            //    Properties = new[] {
            //        new Swampnet.Evl.EventProperty()
            //        {
            //            Name = "one",
            //            Value = "one-value"
            //        }
            //    }
            //};

            //await _eventsRepository.SaveAsync(evt);

            //var x = await _eventsRepository.SearchAsync();
            //foreach(var e in x)
            //{
            //    Console.WriteLine($"[{e.Category}] [{e.Source}] {e.Summary}");
            //}

            await _process.ProcessEventAsync(Guid.Parse("4F2E8EAF-9E31-4B24-8283-CF38FA2B6A88"));
        }
    }
}
