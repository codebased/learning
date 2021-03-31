using System;

namespace Patterns.MultiConfiguration
{
    public class BingService : Service
    {
        public BingService(Func<string, HostConfiguration> configurationResolver)
            : base(configurationResolver, "bing")
        {
        }
    }
}
