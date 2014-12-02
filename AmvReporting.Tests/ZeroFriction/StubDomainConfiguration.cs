using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
