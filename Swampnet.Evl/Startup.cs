using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Swampnet.Evl.Common.Contracts;
using Swampnet.Evl.Services;
using Swampnet.Evl.DAL.InMemory;

namespace Swampnet.Evl
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEventQueueProcessor, EventQueueProcessor>();

            services.AddInMemoryDataProvider();

            // Event Processors
            foreach (var eventProcessor in AppDomain.CurrentDomain.GetAssemblies().AllOfType<IEventProcessor>())
            {
                services.AddSingleton(typeof(IEventProcessor), eventProcessor);
            }

            // Action Handlers
            foreach (var actionHandler in AppDomain.CurrentDomain.GetAssemblies().AllOfType<IActionHandler>())
            {
                services.AddSingleton(typeof(IActionHandler), actionHandler);
            }

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
			});

		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
			loggerFactory.AddSerilog();

			appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseCors(cfg =>
				cfg.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod());

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evl API V1");
			});


			app.UseMvc();
        }
    }
}
