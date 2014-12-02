using System;
using System.Collections.Generic;
using AmvReporting.Infrastructure.NEventStore;
using FluentAssertions;
using Xunit;


namespace AmvReporting.Tests.Infrastructure.NEventStore
{
    public class EventHeaderHelpersTests
    {
        [Fact]
        public void GetCommitDate_EmptyDictionary_ReturnsNull()
        {
            var dictionary = new Dictionary<String, object>();

            var result = dictionary.GetCommitDate();

            result.Should().Be(null);
        }


        [Fact]
        public void GetCommitDate_SavedNotDate_ReturnsNull()
        {
            var dictionary = new Dictionary<String, object>();
            dictionary.Add(MessageHeaders.CommitDate, "blah");

            var result = dictionary.GetCommitDate();

            result.Should().Be(null);
        }


        [Fact]
        public void GetCommitDate_SavedDate_MatchesSavedDate()
        {
            var dictionary = new Dictionary<String, object>();
            var date = new DateTime(2014, 11, 21, 00, 31, 59, 290);
            dictionary.Add(MessageHeaders.CommitDate, date.ToString("dd/MM/yyyy HH:mm:ss.fffffff"));

            var result = dictionary.GetCommitDate();

            result.Should().Be(date);
        }


        [Fact]
        public void GetUsername_EmptyDictionary_ReturnsAnonymous()
        {
            var dictionary = new Dictionary<String, object>();

            var result = dictionary.GetUsername();

            result.Should().Be("Anonymous");
        }


        [Fact]
        public void GetUsername_EmptyString_ReturnsAnonymous()
        {
            var dictionary = new Dictionary<String, object>();
            dictionary.Add(MessageHeaders.Username, "");

            var result = dictionary.GetUsername();

            result.Should().Be("Anonymous");
        }


        [Fact]
        public void GetUsername_StringSaved_MatchesSaved()
        {
            var dictionary = new Dictionary<String, object>();
            var username = Guid.NewGuid().ToString();
            dictionary.Add(MessageHeaders.Username, username);

            var result = dictionary.GetUsername();

            result.Should().Be(username);
        }
    }
}
