using System;
using AmvReporting.Infrastructure.Helpers;
using AmvReporting.Tests.ZeroFriction;
using Xunit;
using Xunit.Extensions;

namespace AmvReporting.Tests.Infrastructure.Helpers
{
    public class ObjectExtensionsTests
    {
        public class TestObject
        {
            public String SomeString { get; set; }
            public int? NullableInt { get; set; }
        }

        [Fact]
        public void CheckForNull_NullObject_ReturnsEmptyString()
        {
            //Arrange
            TestObject testObject = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = testObject.CheckForNull(t => t.SomeString);

            // Assert
            Assert.Empty(result);
        }


        [Theory, AutoDomainData]
        public void CheckForNull_SomeObject_ReturnsString(TestObject testObject)
        {
            //Arrange
            //var testObject = fixture.Create<TestObject>();

            // Act
            var result = testObject.CheckForNull(t => t.SomeString);

            // Assert
            Assert.Equal(testObject.SomeString, result);
        }


        [Fact]
        public void CheckForNull_NullObjectForNullableInt_ReturnsNull()
        {
            //Arrange
            TestObject testObject = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = testObject.CheckForNull(t => t.NullableInt);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public void CheckForNull_IntegerForNullableInt_ReturnsNullableInt()
        {
            //Arrange
            var testObject = new TestObject()
            {
                NullableInt = 3,
            };

            // Act
            var result = testObject.CheckForNull(t => t.NullableInt);

            // Assert
            Assert.Equal(3, result);
        }
    }
}
