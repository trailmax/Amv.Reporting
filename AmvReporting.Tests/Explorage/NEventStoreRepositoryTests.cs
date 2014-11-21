using System;
using AmvReporting.Infrastructure.NEventStore;
using CommonDomain.Persistence;
using CommonDomain.Core;
using FluentAssertions;
using NEventStore;
using Xunit;
using Xunit.Extensions;


namespace AmvReporting.Tests.Explorage
{
    public class NEventStoreRepositoryTests
    {
        [Theory]
        [InlineData(0, null)]
        [InlineData(1, "First")]
        [InlineData(2, "Second")]
        [InlineData(3, "Third")]
        [InlineData(512, "Third")]
        public void Repository_Returns_CorrectVersions(int revision, string expectedName)
        {
            // Arrange
            var sut = new MyEventStoreRepository(WireUpStore(), new AggregateFactory(), new ConflictDetector());
            var id = CreateAggregate(sut);

            // Act
            var firstVersion = sut.GetById<Account>(id, revision);

            // Assert
            firstVersion.Name.Should().Be(expectedName);
        }


        [Fact]
        public void Repository_Gets_CorrectVersions_ManyTimes()
        {
            // Arrange
            var sut = new MyEventStoreRepository(WireUpStore(), new AggregateFactory(), new ConflictDetector());
            var id = CreateAggregate(sut);

            // Act
            var firstVersion = sut.GetById<Account>(id, 1);
            var secondVersion = sut.GetById<Account>(id, 2);
            var thirdVersion = sut.GetById<Account>(id, 3);
            var latestVersion = sut.GetById<Account>(id);

            // Assert
            firstVersion.Name.Should().Be("First");
            secondVersion.Name.Should().Be("Second");
            thirdVersion.Name.Should().Be("Third");
            latestVersion.Name.Should().Be("Third");
        }


        [Fact]
        public void Repository_AfterDisposing_Returns_CorrectVersions()
        {
            // Arrange
            var eventStore = WireUpStore();
            var sut = new MyEventStoreRepository(eventStore, new AggregateFactory(), new ConflictDetector());
            var id = CreateAggregate(sut);
            sut.Dispose();  // disposing just to check if it works
            sut = new MyEventStoreRepository(eventStore, new AggregateFactory(), new ConflictDetector());

            // Act
            var firstVersion = sut.GetById<Account>(id, 1);
            var secondVersion = sut.GetById<Account>(id, 2);
            
            // Assert
            firstVersion.Name.Should().Be("First"); 
            secondVersion.Name.Should().Be("Second");
        }


        [Fact]
        public void When_ApplyingEvents_On_NewInstance_Of_Aggregate_Versions_DontMatch()
        {
            var id = Guid.NewGuid();
            var eventStore = WireUpStore();

            // create aggregate
            var sut = new MyEventStoreRepository(eventStore, new AggregateFactory(), new ConflictDetector());
            var aggregate = new Account(id, "First");
            sut.Save(aggregate, Guid.NewGuid());
            
            // another instance of aggregate
            var newAggregate = sut.GetById<Account>(id);
            newAggregate.ChangeName("Second");
            newAggregate.ChangeName("Third");
            sut.Save(newAggregate, Guid.NewGuid());

            var latestVersion = sut.GetById<Account>(id);

            latestVersion.Name.Should().Be("Third"); // <<-- Fails here. Name value is "First"
        }


        private static Guid CreateAggregate(MyEventStoreRepository sut)
        {
            var id = Guid.NewGuid();
            var aggregate = new Account(id, "First");
            aggregate.ChangeName("Second");
            aggregate.ChangeName("Third");
            sut.Save(aggregate, Guid.NewGuid(), h => { });
            return id;
        }


        private IStoreEvents WireUpStore()
        {
            return Wireup.Init()
                         .UsingInMemoryPersistence()
                         .UsingJsonSerialization()
                         .Build();
        }


        public class Account : AggregateBase
        {
            public String Name { get; private set; }

            private Account(Guid id)
            {
                Id = id;
            }


            public Account(Guid id, string name)
                : this(id)
            {
                RaiseEvent(new ChangeNameEvent(name));
            }

            public void ChangeName(String newName)
            {
                RaiseEvent(new ChangeNameEvent(newName));
            }


            private void Apply(ChangeNameEvent @event)
            {
                this.Name = @event.Name;
            }
        }


        [Serializable]
        public class ChangeNameEvent
        {
            public ChangeNameEvent(string name)
            {
                Name = name;
            }


            public String Name { get; private set; }
        }
    }
}
