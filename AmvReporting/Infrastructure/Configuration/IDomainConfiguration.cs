using System;

namespace AmvReporting.Infrastructure.Configuration
{
    public interface IDomainConfiguration
    {
        string GetRavenDataPath();
        bool EnableRavenStudio();
        String AdministratorRoleNames();
    }
}