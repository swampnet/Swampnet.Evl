﻿using Microsoft.AspNetCore.Builder;
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

namespace Swampnet.Evl
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="appLifetime"></param>
		/// <param name="dal"></param>
		/// <param name="eventProcessor"></param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            //ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime,
            IEventDataAccess dal,
            IEventQueueProcessor eventProcessor)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
				.Enrich.WithExceptionDetails()
				.Enrich.FromLogContext()
                .WriteTo.LocalEvlSink(dal, eventProcessor)
                .WriteTo.Console()
                .CreateLogger();

            //loggerFactory.AddSerilog(); // Pretty noisy!

			appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evl API V1");
				});
            }

            app.UseCors(cfg =>
				cfg.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod());

			app.UseMvc();

            Log.Logger
                .WithTag("START")
                .WithProperty("StartTime", DateTime.UtcNow)
                .Information("Start");
        }
    }

	//class MyOpFil : IOperationFilter
	//{
	//	public void Apply(Operation operation, OperationFilterContext context)
	//	{
	//		if (operation.Parameters == null)
	//		{
	//			operation.Parameters = new List<IParameter>();
	//		}

	//		operation.Parameters.Add(new Peepee()
	//		{
	//			Name = "Foo-Header",
	//			In = "header",
	//			Description = "some description",
	//			Required = false
	//		});
	//	}
	//}

	//class Peepee : IParameter
	//{
	//	public string Name { get; set; }
	//	public string In { get; set; }
	//	public string Description { get; set; }
	//	public bool Required { get; set; }

	//	public Dictionary<string, object> Extensions => null;
	//}
}
