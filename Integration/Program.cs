using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Services;
using Swampnet.Evl.Services.Interfaces;
using System.Threading.Tasks;
using Swampnet.Evl;

namespace Integration
{
    class Program// : IProgram
    {
        private readonly ITest _test;
        private readonly IEventsRepository _eventsRepository;
        private readonly IRuleProcessor _rules;
        private readonly IMaintanence _maintanence;

        static async Task Main(string[] args)
        {
            await Boot().Run();
        }


        public Program(ITest test, IEventsRepository eventsRepository, IRuleProcessor rules, IMaintanence maintanence)
        {
            _test = test;
            _eventsRepository = eventsRepository;
            _rules = rules;
            _maintanence = maintanence;
        }

        
        public async Task Run()
        {
            var id = Guid.NewGuid();

            var evt = new Event()
            {
                Id = id,
                Category = Category.info,
                Source = $"test",
                Summary = $"test-rule-01",
                Properties = new[] {
                        new Property()
                        {
                            Name = "one",
                            Value = "one-value"
                        }
                    },
                Tags = new List<string>()
                {
                    "tag-01",
                    "tag-02"
                }
            };

            //await evt.PostAsync();

            await _eventsRepository.SaveAsync(evt);
            await _rules.ProcessEventAsync(id);

            //for (int i = 0; i < 1; i++)
            //{
            //    var evt = new Swampnet.Evl.Event()
            //    {
            //        Category = Swampnet.Evl.Category.info,
            //        Source = $"test-04",
            //        Summary = $"Test @ {DateTime.UtcNow}",
            //        Properties = new[] {
            //            new Swampnet.Evl.Property()
            //            {
            //                Name = "one",
            //                Value = "one-value"
            //            }
            //        },
            //        Tags = new List<string>()
            //        {
            //            "tag-01",
            //            "tag-02",
            //            "tag-03"
            //        }
            //    };

            //    await _eventsRepository.SaveAsync(evt);
            //}

            //await _maintanence.RunAsync();

            //var x = await _eventsRepository.SearchAsync();
            //foreach (var e in x)
            //{
            //    Console.WriteLine($"[{e.Category}] [{e.Source}] {e.Summary} [{string.Join(",", e.Tags)}]");
            //}

            //await _rules.ProcessEventAsync(Guid.Parse("4F2E8EAF-9E31-4B24-8283-CF38FA2B6A88"));
        }

        private static Program Boot()
        {
            //setup DI
            var serviceProvider = new ServiceCollection()
                .RegisterServiceTypes()
                .AddSingleton<Program>()
                .BuildServiceProvider();

            return serviceProvider.GetService<Program>();
        }
    }
}
