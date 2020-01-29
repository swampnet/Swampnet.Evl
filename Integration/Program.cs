using Microsoft.Extensions.Configuration;
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
        private static IConfiguration _config;
        private readonly ITest _test;
        private readonly IEventsRepository _eventsRepository;

        static async Task Main(string[] args)
        {
            _config = new ConfigurationBuilder()
              .AddJsonFile("settings.json", true, true)
              .AddJsonFile("local.settings.json", true, true)
              .Build();

            //setup DI
            var serviceProvider = new ServiceCollection()
                .RegisterServiceTypes()                
                .AddSingleton<IProgram, Program>()
                .BuildServiceProvider();

            await serviceProvider.GetService<IProgram>().Run();
        }

        public Program(ITest test, IEventsRepository eventsRepository)
        {
            _test = test;
            _eventsRepository = eventsRepository;
        }

        public Task Run()
        {

            _test.Boosh();


            return Task.CompletedTask;

        //    using (var context = new EventsContext(_config.GetConnectionString("events")))
        //    {
        //        var x = context.Events
        //            .Include(f => f.Source)
        //            .Include(f => f.Category)
        //            .Include(f => f.Properties)
        //            .ToArray();

        //        var e = new EventEntity()
        //        {
        //            TimestampUtc = DateTime.UtcNow,
        //            Reference = Guid.NewGuid(),
        //            Summary = "Test",
        //            Category = context.Categories.Single(c => c.Name == "debug"),
        //            Source = context.Sources.Single(c => c.Name == "test"),
        //            Properties = new List<EventPropertyEntity>()
        //            {
        //                new EventPropertyEntity()
        //                {
        //                    Name = "one",
        //                    Value = "one-value"
        //                },
        //                new EventPropertyEntity()
        //                {
        //                    Name = "two",
        //                    Value = "two-value"
        //                },
        //                new EventPropertyEntity()
        //                {
        //                    Name = "three",
        //                    Value = "three-value"
        //                }
        //            }
        //        };

        //        context.Events.Add(e);
        //        context.SaveChanges();
        //    }
        }
    }
}
