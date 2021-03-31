using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Patterns.MultiConfiguration
{
    public static class Execution
    {
        public static void Start(Microsoft.Extensions.Configuration.IConfigurationRoot configuration)
        {
            var serviceProvider = ConfigureServiceProvider(configuration);


            Console.WriteLine(serviceProvider.GetService<GoogleService>().Get());
            Console.WriteLine(serviceProvider.GetService<BingService>().Get());
        }

        private static IServiceProvider ConfigureServiceProvider(IConfigurationRoot configuration)
        {
            var serviceCollection = new ServiceCollection();

            var settings = configuration.GetSection("settings").Get<Settings>();
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
            serviceCollection.AddSingleton<Func<string, HostConfiguration>>(ServiceProvider => key => settings.HostConfigurations[key]);
            serviceCollection.AddSingleton<GoogleService>();
            serviceCollection.AddSingleton<BingService>();

            //setup our DI
            return serviceCollection.BuildServiceProvider();

        }
    }
}