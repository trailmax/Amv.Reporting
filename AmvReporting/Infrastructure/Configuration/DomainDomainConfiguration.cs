using System;
using System.Configuration;

namespace AmvReporting.Infrastructure.Configuration
{
    public class DomainDomainConfiguration : IDomainConfiguration
    {
        public string GetDatabaseConnectionString()
        {
            var connectionString = ConfigurationManager.AppSettings["DatabaseConnectionString"];
            return connectionString;
        }

        public string GetRavenDataPath()
        {
            var dataPath = ConfigurationManager.AppSettings["RavenDataPath"];
            return dataPath;
        }
    }
}