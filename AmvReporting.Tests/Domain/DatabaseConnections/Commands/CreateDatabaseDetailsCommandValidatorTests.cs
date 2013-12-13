using AmvReporting.Domain.DatabaseConnections.Commands;
using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Tests.ZeroFriction;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Domain.DatabaseConnections.Commands
{
    public class CreateDatabaseDetailsCommandValidatorTests
    {
        [Theory, AutoDomainData]
        public void IsValid_ConnectionIsFine_ReturnsTrue(
            [Frozen]IMediator mediator,
            CreateDatabaseDetailsCommandValidator sut, 
            CreateDatabaseDetailsCommand command)
        {
            //Arrange
            var connectionChecking = new ConnectionCheckingData
                                     {
                                         IsConnected = true,
                                     };
            mediator.Request(Arg.Any<CheckDatabaseConnectivityQuery>()).Returns(connectionChecking);

            // Act
            var result = sut.IsValid(command);

            // Assert
            Assert.True(result);
            Assert.Empty(sut.Errors);
        }


        [Theory, AutoDomainData]
        public void IsValid_ConnectionIsBroken_ReturnsFalse(
            [Frozen]IMediator mediator,
            CreateDatabaseDetailsCommandValidator sut,
            CreateDatabaseDetailsCommand command)
        {
            //Arrange
            var connectionChecking = new ConnectionCheckingData
            {
                IsConnected = false,
                ExceptionMessage = "blah-blah"
            };
            mediator.Request(Arg.Any<CheckDatabaseConnectivityQuery>()).Returns(connectionChecking);

            // Act
            var result = sut.IsValid(command);

            // Assert
            Assert.False(result);
            Assert.NotEmpty(sut.Errors);
        }
    }
}
