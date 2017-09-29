using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Plugins.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public static class EmailStartup
    {
        public static void AddEmailActionHandler(this IServiceCollection services)
        {
            services.AddTransient<IActionHandler, EmailActionHandler>();
            services.AddTransient<ITemplateLoader, TemplateLoader>();
            services.AddTransient<ITemplateTransformer, TemplateTransformer>();
        }
    }
}
