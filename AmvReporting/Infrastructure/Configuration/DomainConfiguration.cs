using System;
using System.Configuration;

namespace AmvReporting.Infrastructure.Configuration
{
    public class DomainConfiguration : IConfiguration
    {
        public string GetDatabaseConnectionString()
        {
            var connectionString = ConfigurationManager.AppSettings["DatabaseConnectionString"];
            return connectionString;
        }
    }
}