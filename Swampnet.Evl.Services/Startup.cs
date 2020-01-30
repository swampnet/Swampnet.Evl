using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Implementations;
using Swampnet.Evl.Services.Implementations.EventProcessors;
using Swampnet.Evl.Services.Interfaces;
using System;

namespace Swampnet.Evl.Services
{
    public static class Startup
    {
        public static IServiceCollection RegisterServiceTypes(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("settings.json", true, true)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddTransient<ITest, Test>();
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddTransient<IProcess, ProcessService>();
            services.AddTransient<IMaintanence, Maintanence>();

            services.AddSingleton<IEventProcessor, TestProcessor01>();
            services.AddSingleton<IEventProcessor, TestProcessor02>();
            services.AddSingleton<ITags, TagService>();

            services.AddDbContext<EventsContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("events"));
            });

            return services;
        }
    }
}
