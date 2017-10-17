using Microsoft.Extensions.DependencyInjection;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.DAL.MSSQL.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
