using System;
using AmvReporting.Infrastructure.Helpers;
using FluentAssertions;
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

            expected.Should().Be(result);
        }

        [Fact]
        public void UnescapeString_NewLineText_ConvertsToActualNewLine()
        {
            // Arrange
            const string source = @"""Sql"": ""SELECT TOP 10 *\r\nFROM people\r\n"",";
            var expected = String.Format(@"""Sql"": ""SELECT TOP 10 *{0}FROM people{0}"",", Environment.NewLine);
            // act
            var result = source.Unescape();

            // Assert
            result.Should().Be(expected);
        }


        [Fact]
        public void UnescapeString_Html_ShouldBeHumanReadable()
        {
            // Arrange
            const string source = @"""HtmlOverride"": ""&lt;table class=\""table-report\""&gt;&lt;/table&gt;"",";
            const string expected = @"""HtmlOverride"": ""<table class=""table-report""></table>"",";

            // Act
            var result = source.Unescape();

            // Assert
            result.Should().Be(expected);
        }
    }
}
