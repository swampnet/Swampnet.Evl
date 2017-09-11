﻿using Serilog.Configuration;
using Serilog.Sinks.Evl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog
{
    public static class EvlSinkExtensions
    {
        public static LoggerConfiguration EvlSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  string apiKey,
                  string endpoint,
                  IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new EvlSink(formatProvider, apiKey, endpoint));
        }
    }
}
