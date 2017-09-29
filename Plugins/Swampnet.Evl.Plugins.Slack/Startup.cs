using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Plugins.Slack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public static class Startup
    {
        public static void AddSlackActionHandler(this IServiceCollection services)
        {
            services.AddTransient<IActionHandler, SlackActionHandler>();
        }
    }
}
