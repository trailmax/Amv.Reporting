using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmvReporting.Infrastructure.CQRS;
using NEventStore;


namespace AmvReporting.Domain.Reports.Queries
{
    public class ReportRevision
    {
        public String ActionName { get; set; }

        public object CommitId { get; set; }

        public object DateTime { get; set; }

        public object CommitSequence { get; set; }
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

                foreach (var committedEvent in stream.CommittedEvents)
                {
                    var headers = committedEvent.Headers;
                    object commitSequence;
                    headers.TryGetValue("CommitSequence", out commitSequence);
                    object dateTime;
                    headers.TryGetValue("DateTime", out dateTime);
                    object commitId;
                    headers.TryGetValue("CommitId", out commitId);


                    var reportRevision = new ReportRevision();
                    reportRevision.ActionName = committedEvent.Body.GetType().Name;
                    reportRevision.CommitId = commitId;
                    reportRevision.CommitSequence = commitSequence;
                    reportRevision.DateTime = dateTime;
                    //reportRevision.CommitId = commitId != null ? (Guid?)new Guid((string)commitId) : null;
                    //reportRevision.CommitSequence = commitSequence != null ? (int?)commitSequence : null;
                    //reportRevision.DateTime = dateTime != null ? (DateTime?)dateTime : null;

                    result.Add(reportRevision);
                }

                //List<ReportRevision> result = stream.CommittedEvents
                //                   .Select(e => new ReportRevision()
                //                   {
                //                       ActionName = e.Body.GetType().Name,
                //                       RevisionNumber = e.Headers.TryGetValue(""),
                //                   })
                //                   .ToList();
                return result;


                //var events = stream.CommittedEvents.ToList();
                //foreach (var eventMessage in events)
                //{
                //    var @event = eventMessage.Body;
                //    var eventType = @event.GetType();


                //    //var h = eventMessage.Headers;
                //    //if (h.Count > 0)
                //    //{
                //    //    var hh = h;
                //    //}
                //}
            }

            throw new NotImplementedException();
        }
    }
}