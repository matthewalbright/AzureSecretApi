using System;
using Api.Config.CSServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using Api.Config.Repositories.Utilities;
using Api.Config.Services;
using Api.Config.SRServiceLayer.Interfaces;
using Api.Config.Startup.Middleware;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Api.Config.Startup
{
    public class Startup
    {
        private ILogger Logger { get; set; }

        protected IServiceProvider Container;
        protected IConfigurationRoot Configuration { get; set; }
        protected IHostingEnvironment CurrentEnvironment { get; set; }
        protected ILoggerFactory LoggerFactory { get; set; }

        public const string ServiceName = "Api.Config";
        private const string DefaultCorsPolicy = "DefaultCorsPolicy";

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            CurrentEnvironment = env;
            LoggerFactory = loggerFactory;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.secrets.json", optional: true, reloadOnChange: true)
                .AddJsonFile("authorizationsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            

            CurrentEnvironment = env;

            LoggerFactory = loggerFactory;
            LoggerFactory.AddConsole(Configuration.GetSection("Logging"));
            LoggerFactory.AddDebug();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AddLogging(services, Configuration, LoggerFactory, ServiceName);

            ConfigureSerilog();

            ConfigureDepenencies(services);

            AddCors(services, DefaultCorsPolicy);

            ConfigureServicePointManager();

            services.AddMvc();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMemoryCache();

            services.AddMvcCore();

            ConfigureSwagger(services);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(DefaultCorsPolicy);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            if (!env.IsProduction())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Config API V1");

                });
            }

            app.UseUnknownRoute();
        }


        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = $"{ServiceName} Service",
                        Version = "v1",
                        Description = $"Service for handling {ServiceName} specific back-end operations.",
                        TermsOfService = "API Terms"
                    }
                );
                c.DescribeAllEnumsAsStrings();

                var path = Path.Combine(CurrentEnvironment.ContentRootPath, $"{ServiceName}.Controllers.xml");
                c.IncludeXmlComments(path);

            });

        }

        private void ConfigureDepenencies(IServiceCollection services)
        {
            services.AddScoped<IConfigService, GetConfigService>();
            services.AddScoped(typeof(IRepository), typeof(AzureUtilities));
        }

        private void ConfigureServicePointManager()
        {
            WinHttpHandler httpHandler = new WinHttpHandler()
            {
                SslProtocols = SslProtocols.Tls12,
                MaxConnectionsPerServer = Configuration.GetValue<int>("AppSettings:DefaultConnectionLimit")
            };
        }

        private void ConfigureSerilog()
        {
            var logServiceName = CurrentEnvironment.ApplicationName.Replace(".Startup", string.Empty);
            var exceptionlessApiKey = Configuration.GetSection("Exceptionless:ApiKey").Value.ToString();
            var levelSwitch = new LoggingLevelSwitch();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Exceptionless(exceptionlessApiKey)
                .CreateLogger();
        }

        private static void AddCors(IServiceCollection services, string corsPolicy)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
        }

        private void AddLogging(IServiceCollection services, IConfigurationRoot configuration, ILoggerFactory loggerFactory, string serviceName)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            services.AddScoped(x => loggerFactory.CreateLogger(serviceName));
        }
    }
}
