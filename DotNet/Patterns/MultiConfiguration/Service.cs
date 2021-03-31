using System;

namespace Patterns.MultiConfiguration
{
    public class Service
    {
        private readonly HostConfiguration _configuration;

        public Service(Func<string, HostConfiguration> configurationResolver, string key)
        {
            _configuration = configurationResolver(key);
        }

        public string Get()
        {
            return $"{_configuration.BaseUrl}:{_configuration.Port}";
        }
    }
}
