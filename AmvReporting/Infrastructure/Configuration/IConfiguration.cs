using System;

namespace AmvReporting.Infrastructure.Configuration
{
    public interface IConfiguration
    {
        String GetDatabaseConnectionString();
    }

    public class DomainConfiguration : IConfiguration
    {
        public string GetDatabaseConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}