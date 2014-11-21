using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Helpers;
using AmvReporting.Infrastructure.NEventStore;
using NEventStore;


namespace AmvReporting.Domain.Reports.Queries
{
    public class ReportRevision
    {
        public Guid AggregateId { get; set; }

        public DateTime? CommitDate { get; set; }

        public int RevisionNumber { get; set; }

        public String Username { get; set; }

        public String EventDescription { get; set; }
    }

    public class AllReportRevisionsQuery : IQuery<IEnumerable<ReportRevision>>
    {
        public Guid AggregateId { get; set; }

        public AllReportRevisionsQuery(Guid id)
        {
            AggregateId = id;
        }
    }

    public class AllReportRevisionsQueryHandler : IQueryHandler<AllReportRevisionsQuery, IEnumerable<ReportRevision>>
    {
        private readonly IStoreEvents storeEvents;


        public AllReportRevisionsQueryHandler(IStoreEvents storeEvents)
        {
            this.storeEvents = storeEvents;
        }


        public IEnumerable<ReportRevision> Handle(AllReportRevisionsQuery query)
        {
            using (var stream = storeEvents.OpenStream(query.AggregateId, 0, int.MaxValue))
            {
                var result = new List<ReportRevision>();

                var revisionNumber = 0;
                foreach (var committedEvent in stream.CommittedEvents)
                {
                    revisionNumber++;
                    var headers = committedEvent.Headers;

                    var @event = committedEvent.Body;
                    var reportRevision = new ReportRevision
                    {
                        EventDescription = GetDescription(@event),
                        AggregateId = query.AggregateId,
                        CommitDate = headers.GetCommitDate(),
                        RevisionNumber = revisionNumber,
                        Username = headers.GetUsername(),
                    };

                    result.Add(reportRevision);
                }

                return result;
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