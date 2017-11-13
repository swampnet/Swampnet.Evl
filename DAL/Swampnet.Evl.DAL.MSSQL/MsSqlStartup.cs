using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.DAL.MSSQL.Services;

namespace Swampnet.Evl
{
    public static class MsSqlStartup
    {
        public static void AddSqlServerDataProvider(this IServiceCollection services)
        {
            services.AddSingleton<IRuleDataAccess, RuleDataAccess>();
            services.AddSingleton<IEventDataAccess, EventDataAccess>();
            services.AddSingleton<IManagementDataAccess, ManagementDataAccess>();
        }


        public static void AddEntityFrameworkLogger(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new EFLoggerProvider());
        }
    }
}
