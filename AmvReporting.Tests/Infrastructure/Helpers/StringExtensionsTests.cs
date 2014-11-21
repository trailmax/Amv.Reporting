using System;
using AmvReporting.Infrastructure.Helpers;
using Xunit;
using Xunit.Extensions;


namespace AmvReporting.Tests.Infrastructure.Helpers
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("FirstName", "First Name")]
        [InlineData("FirstLastWord", "First Last Word")]
        [InlineData("PersonId", "Person Id")]
        [InlineData("CRM", "CRM")]
        [InlineData("HR", "HR")]
        [InlineData("POName", "PO Name")]
        [InlineData("OnboardTeam", "Onboard Team")]
        [InlineData("XMLEditor", "XML Editor")]
        public void ToSeparateWords_Separates_Correctly(String source, String expected)
        {
            var result = source.ToSeparatedWords();

            Assert.Equal(expected, result);
        }
    }
}
