using Microsoft.Extensions.Configuration;
using Swampnet.Evl.DAL;
using Swampnet.Evl.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Services;

namespace Integration
{
    interface IProgram 
    {
        void Run();
    }

    class Program : IProgram
    {
        private static IConfiguration _config;

        static void Main(string[] args)
        {
            _config = new ConfigurationBuilder()
              .AddJsonFile("settings.json", true, true)
              .AddJsonFile("local.settings.json", true, true)
              .Build();

            //setup DI
            var serviceProvider = new ServiceCollection()
                .RegisterTypes()                
                .AddSingleton<IProgram, Program>()
                .BuildServiceProvider();

            serviceProvider.GetService<IProgram>().Run();
        }

        public void Run()
        {
            using (var context = new EventsContext(_config.GetConnectionString("events")))
            {
                var x = context.Events
                    .Include(f => f.Source)
                    .Include(f => f.Category)
                    .Include(f => f.Properties)
                    .ToArray();

                var e = new Event()
                {
                    TimestampUtc = DateTime.UtcNow,
                    Reference = Guid.NewGuid(),
                    Summary = "Test",
                    Category = context.Categories.Single(c => c.Name == "debug"),
                    Source = context.Sources.Single(c => c.Name == "test"),
                    Properties = new List<EventProperty>()
                    {
                        new EventProperty()
                        {
                            Name = "one",
                            Value = "one-value"
                        },
                        new EventProperty()
                        {
                            Name = "two",
                            Value = "two-value"
                        },
                        new EventProperty()
                        {
                            Name = "three",
                            Value = "three-value"
                        }
                    }
                };

                context.Events.Add(e);
                context.SaveChanges();
            }
        }
    }
}
