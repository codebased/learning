using System;

namespace Patterns.MultiConfiguration
{
    public class GoogleService : Service
    {
        public GoogleService(Func<string, HostConfiguration> configurationResolver)
            : base(configurationResolver, "google")
        {
        }
    }
}
