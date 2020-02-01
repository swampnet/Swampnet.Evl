using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Services.DAL;
using Swampnet.Evl.Services.Implementations;
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
            services.AddTransient<ITags, TagService>();
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddTransient<IRuleProcessor, RuleProcessor>();
            services.AddTransient<IMaintanence, Maintanence>();

            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.AddTagAction>();
            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.RemoveTagAction>();
            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.SetCategoryAction>();
            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.AddPropertyAction>();

            services.AddDbContext<EventsContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("events"));
            });

            return services;
        }
    }
}
