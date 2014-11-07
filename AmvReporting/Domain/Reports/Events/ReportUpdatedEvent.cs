//using System;
//using System.Web;
//using AmvReporting.Domain.Reports.Queries;
//using AmvReporting.Infrastructure.Events;


//namespace AmvReporting.Domain.Reports.Events
//{
//    public class ReportUpdatedEvent : IDomainEvent
//    {
//        public string Id { get; private set; }


//        public ReportUpdatedEvent(String id)
//        {
//            this.Id = id;
//        }
//    }

//    public class InvalidateCacheReportUpdatedEventHandler : IDomainEventHandler<ReportUpdatedEvent>
//    {
//        public void Handle(ReportUpdatedEvent raisedEvent)
//        {
//            var query = new ReportResultQuery(raisedEvent.Id);
//            HttpContext.Current.Cache.Remove(query.CacheKey);
//        }
//    }
//}