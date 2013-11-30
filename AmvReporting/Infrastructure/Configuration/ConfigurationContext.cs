using System;

namespace AmvReporting.Infrastructure.Configuration
{
    public static class ConfigurationContext
    {
        private static IConfiguration configuration;

        public static IConfiguration Current
        {
            get
            {
                if (configuration == null)
                {
                    configuration = new DomainConfiguration();
                }
                return configuration;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("configuration");
                }
                configuration = value;
            }
        }

        public static void ResetToDefault()
        {
            configuration = new DomainConfiguration();
        }
    }
}