using System;
using AmvReporting.Domain.Reports.Queries;
using FluentAssertions;
using Xunit;


namespace AmvReporting.Tests.Domain.Reports.Queries
{
    public class CompareToLatestQueryHandlerTests
    {
        [Fact]
        public void UnescapeString_NewLineText_ConvertsToActualNewLine()
        {
            // Arrange
            var source = @"""Sql"": ""SELECT TOP 10 *\r\nFROM people\r\n"",";
            var expected = String.Format(@"""Sql"": ""SELECT TOP 10 *{0}FROM people{0}"",", Environment.NewLine);
            // act
            var result = CompareToLatestQueryHandler.UnescapeString(source);

            // Assert
            result.Should().Be(expected);
        }


        [Fact]
        public void UnescapeString_Html_ShouldBeHumanReadable()
        {
            // Arrange
            var source = @"""HtmlOverride"": ""&lt;table class=\""table-report\""&gt;&lt;/table&gt;"",";
            var expected = @"""HtmlOverride"": ""<table class=""table-report""></table>"",";

            // Act
            var result = CompareToLatestQueryHandler.UnescapeString(source);

            // Assert
            result.Should().Be(expected);
        }
    }
}
