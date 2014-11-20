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
        public int RevisionNumber { get; set; }
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
                var result = stream.CommittedEvents
                                   .Select(e => new ReportRevision()
                                   {
                                       ActionName = e.Body.GetType().Name,
                                       RevisionNumber = 0,
                                   })
                                   .ToList();
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