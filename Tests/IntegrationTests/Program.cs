using Serilog;
using Serilog.Exceptions;
using System;
using System.Threading;
using Swampnet.Evl;
using Microsoft.Extensions.Configuration;
using System.IO;
using Swampnet.Evl.Client;

namespace IntegrationTests
{
    static class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("cfg.json");

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console()
                .WriteTo.EvlSink(
                    Configuration["evl:api-key"], 
                    Configuration["evl:endpoint"])
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
			Random rnd = new Random();

            while (true)
            {
                try
                {
                    //Log.Information("Some Properties {Count} {One} {Two} ", count++, 1, 2);

                    if (count % 10 == 0)
                    {
                        throw new Exception("Text Exception");
                    }
                    else if(count %7 == 0)
                    {
                        throw new Exception("Text Exception NOT-AN-ERROR");
                    }
					else if(count % 13 == 0)
					{
						//Log.Logger
						//	.WithTags(new[] { "INTEGRATION-TEST" })
						//	.WithTags(new[] { "TAG-01", "TAG-02" })
						//	.WithProperty("xxx", "yyy")
						//	.WithProperty("~TAG~", "TAG-03", "~TAG~")
						//	.WithProperties(new[]
						//	{
						//		new Property("Additional Property", "value 1"),
						//		new Property("Another Additional Property", "value 2")
						//	}).Information("Some inline properties {Count} {One} {Two} ", count++, 1, 2);

						var l = Log.Logger
							.WithTags(new[] { "INTEGRATION-TEST" })
							.WithProperties(new[]
							{
								new Property("Additional Property", "value 1"),
								new Property("Another Additional Property", "value 2")
							});

						if (rnd.NextDouble() > 0.5)
						{
							l = l.WithTag("TAG-01");
						}
						if (rnd.NextDouble() > 0.5)
						{
							l = l.WithTag("TAG-02");
						}

						l.Information("Some inline properties {Count} {One} {Two} ", count++, 1, 2);
					}
                }
                catch (Exception ex)
                {
                    ex.AddData("Count", count);
                    Log.Error(ex, ex.Message);
                }
                finally
                {
                    Thread.Sleep(2000);
					count++;
                }
            }
        }
    }
}
