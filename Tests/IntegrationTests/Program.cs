using Serilog;
using System;
using System.Threading;

namespace IntegrationTests
{
    static class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.EvlSink()
                .CreateLogger();

            int count = 1;
            while (true)
            {
                Log.Information("Some Properties {One} {Two} {Count}", 1, 2, count++);
                Thread.Sleep(5000);
            }

            Console.WriteLine("key");
            Console.ReadKey(true);
            Log.CloseAndFlush();
        }
    }
}
