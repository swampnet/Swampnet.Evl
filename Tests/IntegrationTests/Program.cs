using Serilog;
using Serilog.Exceptions;
using System;
using System.Threading;
using Swampnet.Evl;

namespace IntegrationTests
{
    static class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console()
                .WriteTo.EvlSink()
                .CreateLogger();

            //LogException();
            RaiseEvents();

            Console.WriteLine("key");
            Console.ReadKey(true);
            Log.CloseAndFlush();
        }


        private static void LogException()
        {
            try
            {
                throw new Exception("Test exception");
            }
            catch (Exception ex)
            {
                ex.AddData("key-one", 1);
                ex.AddData("key-two", 2);

                Log.Error(ex, ex.Message);
            }
        }


        private static void RaiseEvents()
        {
            int count = 1;
            while (true)
            {
                try
                {
                    Log.Information("Some Properties {Count} {One} {Two} ", count++, 1, 2);

                    if (count % 10 == 0)
                    {
                        throw new Exception("Text Exception");
                    }
                    else if(count %7 == 0)
                    {
                        throw new Exception("Text Exception NOT-AN-ERROR");
                    }
                }
                catch (Exception ex)
                {
                    ex.AddData("Count", count);
                    Log.Error(ex, ex.Message);
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
