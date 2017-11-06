using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public static class LoggerExtensions
    {
        public static ILogger WithProperties(this ILogger logger, object source)
        {
            return logger.WithProperties(source.GetPublicProperties());
        }
    }
}
