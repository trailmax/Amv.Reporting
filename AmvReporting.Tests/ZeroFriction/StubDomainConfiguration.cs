using System;
using AmvReporting.Infrastructure.Configuration;

namespace AmvReporting.Tests.ZeroFriction
{
    public class StubDomainConfiguration : IDomainConfiguration
    {
        public string GetRavenDataPath()
        {
            return @"d:\Raven\test\";
        }


        public string GetBackupPath()
        {
            throw new NotSupportedException();
        }


        public bool EnableRavenStudio()
        {
            return false;
        }


        public string AdministratorRoleNames()
        {
            throw new NotSupportedException();
        }


        public string GetGoogleAnalyticsCode()
        {
            throw new NotSupportedException();
        }

        public bool RequireHttps()
        {
            throw new NotImplementedException();
        }
    }
}
