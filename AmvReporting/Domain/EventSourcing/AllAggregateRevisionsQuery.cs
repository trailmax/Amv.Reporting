using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using AmvReporting.Infrastructure.NEventStore;
using NEventStore;


namespace AmvReporting.Domain.EventSourcing
{
    public class AggregateRevision
    {
        public Guid AggregateId { get; set; }

        public DateTime? CommitDate { get; set; }

        public int RevisionNumber { get; set; }

        public String Username { get; set; }

        public String EventDescription { get; set; }
    }

    public class AllAggregateRevisionsQuery : IQuery<IEnumerable<AggregateRevision>>
    {
        public Guid AggregateId { get; set; }

        public AllAggregateRevisionsQuery(Guid id)
        {
            AggregateId = id;
        }
    }

    public class AllAggregateRevisionsQueryHandler : IQueryHandler<AllAggregateRevisionsQuery, IEnumerable<AggregateRevision>>
    {
        private readonly IStoreEvents storeEvents;


        public AllAggregateRevisionsQueryHandler(IStoreEvents storeEvents)
        {
            this.storeEvents = storeEvents;
        }


        public IEnumerable<AggregateRevision> Handle(AllAggregateRevisionsQuery query)
        {
            using (var stream = storeEvents.OpenStream(query.AggregateId, 0, int.MaxValue))
            {
                var result = new List<AggregateRevision>();

                var revisionNumber = 0;
                foreach (var committedEvent in stream.CommittedEvents)
                {
                    revisionNumber++;
                    var headers = committedEvent.Headers;

                    var @event = committedEvent.Body;
                    var revision = new AggregateRevision
                    {
                        EventDescription = GetDescription(@event),
                        AggregateId = query.AggregateId,
                        CommitDate = headers.GetCommitDate(),
                        RevisionNumber = revisionNumber,
                        Username = headers.GetUsername(),
                    };

                    result.Add(revision);
                }

                return result.OrderByDescending(r => r.RevisionNumber);
            }
        }


        private String GetDescription(object @event)
        {
            var attribute = @event.GetType().GetCustomAttribute<DescriptionAttribute>();
            if (attribute == null)
            {
                return @event.GetType().Name.ToSeparatedWords().LowerCasePrepositions();
            }

            return attribute.Description;
        }
    }
}