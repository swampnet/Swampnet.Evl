using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.DAL.InMemory.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.InMemory
{
    public static class Extensions
    {
        public static void AddInMemoryDataProvider(this IServiceCollection services)
        {
            services.AddSingleton<IRuleDataAccess, RuleDataAccess>();
            services.AddSingleton<IEventDataAccess, EventDataAccess>();
        }
    }
}
