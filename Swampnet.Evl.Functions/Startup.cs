using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swampnet.Evl.Functions.Interfaces;
using Swampnet.Evl.Functions.Services;

[assembly: FunctionsStartup(typeof(Swampnet.Evl.Functions.Startup))]
namespace Swampnet.Evl.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITest, Test>();
        }
    }
}
