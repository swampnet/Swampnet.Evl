using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Services.Implementations;
using Swampnet.Evl.Services.Interfaces;
using System;

namespace Swampnet.Evl.Services
{
    public static class X
    {
        public static IServiceCollection RegisterTypes(this IServiceCollection services)
        {
            services.AddSingleton<ITest, Test>();

            return services;
        }
    }
}
