using System;

namespace AmvReporting.Infrastructure.Configuration
{
    public interface IDomainConfiguration
    {
        String GetDatabaseConnectionString();
        string GetRavenDataPath();
        bool EnableRavenStudio();
    }
}