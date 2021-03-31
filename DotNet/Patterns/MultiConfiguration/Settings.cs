using System.Collections.Generic;

namespace Patterns.MultiConfiguration
{
    public class Settings
    {
        public IDictionary<string, HostConfiguration> HostConfigurations { get; set; }
    }
}