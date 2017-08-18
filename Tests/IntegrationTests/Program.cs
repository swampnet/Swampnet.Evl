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

            int count = 1;
            while (true)
            {
                try
                {
                    Log.Information("Some Properties {Count} {One} {Two} ", count++, 1, 2);

                    if(count % 10 == 0)
                    {
                        throw new Exception("Text Exception");
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

            Console.WriteLine("key");
            Console.ReadKey(true);
            Log.CloseAndFlush();
        }
    }
}
