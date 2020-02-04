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

            services.AddSingleton<IConfigurationRoot>(config);

            services.AddTransient<ITest, Test>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ISourceRepository, SourceRepository>();
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddTransient<IRuleRepository, RuleRepository>();
            services.AddTransient<IRuleProcessor, RuleProcessor>();
            services.AddTransient<IMaintanence, Maintanence>();
            services.AddTransient<INotify, NotifyService>();

            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.AddTagAction>();
            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.RemoveTagAction>();
            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.SetCategoryAction>();
            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.AddPropertyAction>();
            services.AddTransient<IActionProcessor, Implementations.ActionProcessors.EmailAction>();

            services.AddDbContext<EventsContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("events"));
            });

            return services;
        }
    }
}
