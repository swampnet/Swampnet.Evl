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
			});
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            Log.Logger.WithTags(new[] { "START" }).Information("Start");
        }
    }
}
