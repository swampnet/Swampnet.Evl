using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Swampnet.Evl.Services;
using Swampnet.Evl.Contracts;
using Swampnet.Evl.Common.Contracts;
using System;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection.Metadata;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace Swampnet.Evl
{
    /// <summary>
    /// Swampnet.Evl startup
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Called by the runtime
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEventQueueProcessor, EventQueueProcessor>();

            //services.AddInMemoryDataProvider();
            services.AddSqlServerDataProvider();

            // Add default Event Processors
            services.AddDefaultEventProcessors();

            // Add default Action Handlers
            services.AddDefaultActionHandlers();

            services.AddEmailActionHandler();
            services.AddSlackActionHandler();

            services.AddSingleton<IAuth, Auth>();
            services.AddSingleton<IHostedService, TruncateDataHostedService>();

            // Add framework services.  
            services.AddMvc().AddJsonOptions(options => {
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});


			services.AddCors();

			// Register the Swagger generator, defining one or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Title = "Evl API",
					Version = "v1",
					Description = "Backend API for Evl"					
				});


				var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Swampnet.Evl.xml");
				c.IncludeXmlComments(filePath);

				//c.OperationFilter<MyOpFil>();
			});
		}


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        public void Configure(
            IApplicationBuilder app,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            Microsoft.AspNetCore.Hosting.IApplicationLifetime appLifetime,
            IEventDataAccess dal,
            IConfiguration cfg,
            IEventQueueProcessor eventProcessor)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
				.Enrich.WithExceptionDetails()
				.Enrich.FromLogContext()
                .WriteTo.LocalEvlSink(
                    Guid.Parse(cfg["evl:org-id"]),
                    dal,
                    eventProcessor,
                    cfg["evl:source"] ?? typeof(Startup).Assembly.GetName().Name,
                    typeof(Startup).Assembly.GetName().Version.ToString())
                .WriteTo.Console()
                .CreateLogger();

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

			if (env.IsDevelopment())
            {
                //loggerFactory.AddSerilog(); // Pretty noisy!
                loggerFactory.AddEntityFrameworkLogger();

                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evl API V1");
                });
            }


            app.UseCors(c =>
				c.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod());

            app.UseStaticFiles();

			app.UseMvc();

            Log.Logger
                .WithTag("START")
                .WithProperty("StartTime", DateTime.UtcNow)
                .Information("Start");
        }
    }
}
