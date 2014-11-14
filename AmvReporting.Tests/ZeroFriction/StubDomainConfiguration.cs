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
            throw new NotSupportedException();
        }


        public string GetBackupPath()
        {
            throw new NotSupportedException();
        }


        public bool EnableRavenStudio()
        {
            throw new NotSupportedException();
        }


        public string AdministratorRoleNames()
        {
            throw new NotSupportedException();
        }
    }
}
