using System;
using System.Collections.Generic;
using System.Linq;
using AmvReporting.Infrastructure.Helpers;
using CommonDomain;
using CommonDomain.Persistence;
using NEventStore;
using NEventStore.Persistence;


namespace AmvReporting.Infrastructure.NEventStore
{
    public class MyEventStoreRepository : IRepository
	{
		private const string AggregateTypeHeader = "AggregateType";

		private readonly IDetectConflicts conflictDetector;

		private readonly IStoreEvents eventStore;

		private readonly IConstructAggregates factory;

		private readonly IDictionary<string, ISnapshot> snapshots = new Dictionary<string, ISnapshot>();

		private readonly IDictionary<string, IEventStream> streams = new Dictionary<string, IEventStream>();

        public MyEventStoreRepository(IStoreEvents eventStore, IConstructAggregates factory, IDetectConflicts conflictDetector)
		{
			this.eventStore = eventStore;
			this.factory = factory;
			this.conflictDetector = conflictDetector;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
		{
			return GetById<TAggregate>(Bucket.Default, id);
		}

		public virtual TAggregate GetById<TAggregate>(Guid id, int versionToLoad) where TAggregate : class, IAggregate
		{
			return GetById<TAggregate>(Bucket.Default, id, versionToLoad);
		}

		public TAggregate GetById<TAggregate>(string bucketId, Guid id) where TAggregate : class, IAggregate
		{
			return GetById<TAggregate>(bucketId, id, int.MaxValue);
		}

		public TAggregate GetById<TAggregate>(string bucketId, Guid id, int versionToLoad) where TAggregate : class, IAggregate
		{
			ISnapshot snapshot = GetSnapshot(bucketId, id, versionToLoad);
			IEventStream stream = OpenStream(bucketId, id, versionToLoad, snapshot);
			IAggregate aggregate = GetAggregate<TAggregate>(snapshot, stream);

			ApplyEventsToAggregate(versionToLoad, stream, aggregate);

			return aggregate as TAggregate;
		}

		public virtual void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
		{
			Save(Bucket.Default, aggregate, commitId, updateHeaders);
		}

		public void Save(string bucketId, IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
		{
			Dictionary<string, object> headers = PrepareHeaders(aggregate, updateHeaders);
			while (true)
			{
				IEventStream stream = PrepareStream(bucketId, aggregate, headers);
				int commitEventCount = stream.CommittedEvents.Count;

				try
				{
					stream.CommitChanges(commitId);
					aggregate.ClearUncommittedEvents();
					return;
				}
				catch (DuplicateCommitException)
				{
					stream.ClearChanges();
					return;
				}
				catch (ConcurrencyException e)
				{
                    var conflict = ThrowOnConflict(stream, commitEventCount);
                    stream.ClearChanges();

                    if (conflict)
					{
						throw new ConflictingCommandException(e.Message, e);
					}
				}
				catch (StorageException e)
				{
					throw new PersistenceException(e.Message, e);
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			lock (streams)
			{
				foreach (var stream in streams)
				{
					stream.Value.Dispose();
				}

				snapshots.Clear();
				streams.Clear();
			}
		}

		private static void ApplyEventsToAggregate(int versionToLoad, IEventStream stream, IAggregate aggregate)
		{
            if (versionToLoad == 0 || aggregate.Version < versionToLoad)
            {
                var events = stream.CommittedEvents.ToArray();
                var topVersion = Math.Min(events.Count(), versionToLoad);

                for (int ver = 0; ver < topVersion; ver++)
                {
                    var @event = events[ver].Body;
                    aggregate.ApplyEvent(@event);
                }
            }
		}

		private IAggregate GetAggregate<TAggregate>(ISnapshot snapshot, IEventStream stream)
		{
			IMemento memento = snapshot == null ? null : snapshot.Payload as IMemento;
			return factory.Build(typeof(TAggregate), stream.StreamId.ToGuid(), memento);
		}

		private ISnapshot GetSnapshot(string bucketId, Guid id, int version)
		{
			ISnapshot snapshot;
			var snapshotId = bucketId + id;
			if (!snapshots.TryGetValue(snapshotId, out snapshot))
			{
				snapshots[snapshotId] = snapshot = eventStore.Advanced.GetSnapshot(bucketId, id, version);
			}

			return snapshot;
		}

		private IEventStream OpenStream(string bucketId, Guid id, int version, ISnapshot snapshot)
		{
			IEventStream stream;
			var streamId = bucketId + "+" + id + "+" + version;
			if (streams.TryGetValue(streamId, out stream))
			{
				return stream;
			}

			stream = snapshot == null 
                ? eventStore.OpenStream(bucketId, id, 0, version)
				: eventStore.OpenStream(snapshot, version);

		    var eventStream = streams[streamId] = stream;
		    return eventStream;
		}

		private IEventStream PrepareStream(string bucketId, IAggregate aggregate, Dictionary<string, object> headers)
		{
			IEventStream stream;
			var streamId = bucketId + "+" + aggregate.Id + "+" + int.MaxValue;
			if (!streams.TryGetValue(streamId, out stream))
			{
				streams[streamId] = stream = eventStore.CreateStream(bucketId, aggregate.Id);
			}

			foreach (var item in headers)
			{
				stream.UncommittedHeaders[item.Key] = item.Value;
			}

			aggregate.GetUncommittedEvents()
			         .Cast<object>()
			         .Select(x => new EventMessage { Body = x })
			         .ToList()
			         .ForEach(stream.Add);

			return stream;
		}

		private static Dictionary<string, object> PrepareHeaders(
			IAggregate aggregate, Action<IDictionary<string, object>> updateHeaders)
		{
			var headers = new Dictionary<string, object>();

			headers[AggregateTypeHeader] = aggregate.GetType().FullName;
			if (updateHeaders != null)
			{
				updateHeaders(headers);
			}

			return headers;
		}

		private bool ThrowOnConflict(IEventStream stream, int skip)
		{
			IEnumerable<object> committed = stream.CommittedEvents.Skip(skip).Select(x => x.Body);
			IEnumerable<object> uncommitted = stream.UncommittedEvents.Select(x => x.Body);
			return conflictDetector.ConflictsWith(uncommitted, committed);
		}
	}
}