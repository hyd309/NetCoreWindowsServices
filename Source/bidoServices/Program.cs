using System;
using System.Collections.Generic;
using PeterKottas.DotNetCore.WindowsService;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace bidoServices
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot Configuration;
            // ILoggerFactory LoggerFactory;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddOptions();
            //services.Configure<ConfigModel>(Configuration.GetSection("ConfigSetting"));
            //services.AddSingleton<IConfiguration>(Configuration);
            
            services.AddTransient<IBaseService, SyncService>();

            var serviceProvider = services.BuildServiceProvider();

            ServiceRunner<ServiceFactory>.Run(config =>
            {
                var name = config.GetDefaultName();
                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        return new ServiceFactory(controller, serviceProvider.GetService<IEnumerable<IBaseService>>());
                    });
                    serviceConfig.OnStart((service, extraArguments) =>
                    {
                        Console.WriteLine("Service {0} started", name);
                        service.Start();
                    });

                    serviceConfig.OnStop(service =>
                    {
                        Console.WriteLine("Service {0} stopped", name);
                        service.Stop();
                    });

                    serviceConfig.OnError(e =>
                    {
                        Console.WriteLine("Service {0} errored with exception : {1}", name, e.Message);
                    });
                });

                config.SetName("SAASService");
                config.SetDescription("SAAS Service For All Saas Client");
                config.SetDisplayName("SAAS Service");
            });
        }
    }
}
