using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Swampnet.Evl.DAL.MSSQL.Services
{
    class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command")
            {
                return new EFLogger();
            }

            return new NullLogger();
        }


        public void Dispose()
        {
        }


        private class EFLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }


            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Debug.WriteLine(formatter(state, exception));
            }


            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }

        private class NullLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return false;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            { }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
