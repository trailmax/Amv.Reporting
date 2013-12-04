using System;
using AmvReporting.Infrastructure.Helpers;
using Xunit;

namespace AmvReporting.Tests.Infrastructure.Helpers
{
    public class ExpressionHelperTests
    {
        [Fact]
        public void PropertyName_Always_NameMatches()
        {
            Assert.Equal("Name", ExpressionHelper.PropertyName<ClassOne>(t => t.Name));
            Assert.Equal("FullName", ExpressionHelper.PropertyName<ClassOne>(t => t.FullName));
        }

        [Fact]
        public void PropertyValue_Always_EvaluatesExpression()
        {
            var @object = new ClassOne() { Name = "hah", FullName = "Hey, buddy!" };

            Assert.Equal("hah", ExpressionHelper.PropertyValue(@object, o => o.Name));
            Assert.Equal("Hey, buddy!", ExpressionHelper.PropertyValue(@object, o => o.FullName));
        }
    }


    internal class ClassOne
    {
        public String Name { get; set; }
        public String FullName { get; set; }
    }
}
