using System;
using System.Configuration;

namespace AmvReporting.Infrastructure.Configuration
{
    public class DomainConfiguration : IDomainConfiguration
    {
        public string GetRavenDataPath()
        {
            var dataPath = ConfigurationManager.AppSettings["RavenDataPath"];
            return dataPath;
        }

        public bool EnableRavenStudio()
        {
            var result = String.Equals(ConfigurationManager.AppSettings["EnableRavenStudio"], "true", StringComparison.InvariantCultureIgnoreCase);

            return result;
        }

        public String AdministratorRoleNames()
        {
            var result = ConfigurationManager.AppSettings["AdministratorRoleNames"];

            return result;
        }
    }
}