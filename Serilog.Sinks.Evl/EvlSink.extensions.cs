using Serilog.Configuration;
using Serilog.Sinks.Evl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog
{
    public static class EvlSinkExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="apiKey"></param>
        /// <param name="endpoint"></param>
        /// <param name="source">Event source. Defaults to executing assembly name</param>
        /// <param name="sourceVersion">Event source version. Defaults to executing assembly version</param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static LoggerConfiguration EvlSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  string apiKey,
                  string endpoint,
                  string source = null,
                  string sourceVersion = null,
                  IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new EvlSink(formatProvider, apiKey, endpoint, source, sourceVersion));
        }
    }
}
