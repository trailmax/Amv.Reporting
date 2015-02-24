using System;

namespace AmvReporting.Infrastructure.Configuration
{
    public interface IDomainConfiguration
    {
        string GetRavenDataPath();
        string GetBackupPath();
        bool EnableRavenStudio();
        String AdministratorRoleNames();
        String GetGoogleAnalyticsCode();
        bool RequireHttps();
    }
}