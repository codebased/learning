using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patterns.MultiConfiguration;

namespace Patterns
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            Execution.Start(configuration);

       
        }

       
    }
}
