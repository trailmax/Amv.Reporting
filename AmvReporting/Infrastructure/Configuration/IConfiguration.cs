using System;

namespace AmvReporting.Infrastructure.Configuration
{
    public interface IConfiguration
    {
        String GetDatabaseConnectionString();
    }
}