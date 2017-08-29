using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Plugins.Email
{
    public static class Startup
    {
        public static void AddEmailActionHandler(this IServiceCollection services)
        {
            services.AddSingleton<IActionHandler, EmailActionHandler>();

            services.AddTransient<ITemplateLoader, TemplateLoader>();
            services.AddTransient<ITemplateTransformer, TemplateTransformer>();
        }
    }
}
