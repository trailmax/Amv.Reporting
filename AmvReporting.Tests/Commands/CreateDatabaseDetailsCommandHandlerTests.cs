using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmvReporting.Commands;
using AmvReporting.Tests.ZeroFriction;
using Xunit.Extensions;

namespace AmvReporting.Tests.Commands
{
    public class CreateDatabaseDetailsCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public void CreateModel_Always_MapsAllProperties(CreateDatabaseDetailsCommand command, CreateDatabaseDetailsCommandHandler sut)
        {
            var result = sut.CreateModel(command);

            AssertionHelpers.PropertiesAreEqual(command, result);
        }
    }
}
