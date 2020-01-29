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
            services.AddTransient<ITest, Test>();
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddDbContext<EventsContext>();

            return services;
        }
    }
}
