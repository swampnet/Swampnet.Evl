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
        //private static IConfiguration _config;
        private readonly ITest _test;
        private readonly IEventsRepository _eventsRepository;

        static async Task Main(string[] args)
        {
            //_config = new ConfigurationBuilder()
            //  .AddJsonFile("settings.json", true, true)
            //  .AddJsonFile("local.settings.json", true, true)
            //  .Build();

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

        public async Task Run()
        {
            await _eventsRepository.SaveAsync(new Swampnet.Evl.Event() { 
                Category = Swampnet.Evl.Category.debug,
                Source = "test",
                Summary = $"Test @ {DateTime.UtcNow}",
                Properties = new[] { 
                    new Swampnet.Evl.EventProperty()
                    {
                        Name = "one",
                        Value = "one-value"
                    }
                }
            });

            var x = await _eventsRepository.SearchAsync();
            foreach(var e in x)
            {
                Console.WriteLine($"{e.Summary}");
            }
        }
    }
}
