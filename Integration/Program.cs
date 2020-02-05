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
using System.Threading;

namespace Integration
{
    class Program// : IProgram
    {
        private readonly ITest _test;
        private readonly IEventsRepository _eventsRepository;
        private readonly IRuleProcessor _rules;
        private readonly IMaintanence _maintanence;
        private readonly INotify _notify;
        private readonly IRuleRepository _ruleRepository;

        static async Task Main(string[] args)
        {
            await Boot().Run();
        }


        public Program(
            IConfigurationRoot cfg,
            ITest test,
            IEventsRepository eventsRepository,
            IRuleProcessor rules,
            IMaintanence maintanence,
            INotify notify,
            IRuleRepository ruleRepository)
        {
            _test = test;
            _eventsRepository = eventsRepository;
            _rules = rules;
            _maintanence = maintanence;
            _notify = notify;
            _ruleRepository = ruleRepository;
        }

        
        public async Task Run()
        {
            //await _notify.SendEmailAsync(new Swampnet.Evl.EmailMessage() 
            //{ 
            //    Subject = "Test subject",
            //    Body = "<body>TEST BODY</body>",
            //    To = new List<EmailMessage.Recipient>() { 
            //        new EmailMessage.Recipient("pj@theswamp.co.uk")
            //    }
            //});

            //var id = Guid.NewGuid();

            //var evt = new Event()
            //{
            //    Id = id,
            //    Category = Category.info,
            //    Source = $"test-02",
            //    Summary = $"Test notifications",
            //    Properties = new[] {
            //            new Property()
            //            {
            //                Name = "one",
            //                Value = "one-value"
            //            }
            //        },
            //    Tags = new List<string>()
            //    {
            //        "test-email"
            //    }
            //};

            //var evt = new Event()
            //{
            //    Id = id,
            //    Category = Category.info,
            //    Source = $"test-02",
            //    Summary = $"test-rule-01",
            //    Properties = new[] {
            //            new Property()
            //            {
            //                Name = "one",
            //                Value = "one-value"
            //            }
            //        },
            //    Tags = new List<string>()
            //    {
            //        "tag-01",
            //        "tag-02",
            //        "tag-03"
            //    }
            //};

            //await evt.PostAsync();

            //await _eventsRepository.SaveAsync(evt);

            //while (true)
            //{
            //    Console.WriteLine(DateTime.Now);
            //    await _rules.ProcessEventAsync(id);
            //    Thread.Sleep(10000);
            //}

            //var evt = new Event()
            //{
            //    Id = id,
            //    Category = Category.info,
            //    Source = $"test-02",
            //    Summary = $"test-rule-01",
            //    Properties = new[] {
            //            new Property()
            //            {
            //                Name = "one",
            //                Value = "one-value"
            //            }
            //        },
            //    Tags = new List<string>()
            //    {
            //        "tag-01",
            //        "tag-02",
            //        "tag-03"
            //    }
            //};

            //await evt.PostAsync();

            //await _eventsRepository.SaveAsync(evt);
            //await _rules.ProcessEventAsync(id);

            //var x = await _eventsRepository.LoadAsync(id);

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

            var x = await _eventsRepository.SearchAsync(new EventSearchCriteria()
            {
                PageSize = 10,
                Page = 1,
                ShowDebug = false,
                ShowError = true,
                ShowInformation = false,
                Tags = "taggy"
            });

            //foreach (var e in x)
            //{
            //    Console.WriteLine($"[{e.Category}] [{e.Source}] {e.Summary} [{string.Join(",", e.Tags)}]");
            //}

            //await _rules.ProcessEventAsync(Guid.Parse("4F2E8EAF-9E31-4B24-8283-CF38FA2B6A88"));

            //var rules = await _ruleRepository.LoadRulesAsync();
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
