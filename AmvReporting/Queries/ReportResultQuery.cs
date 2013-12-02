using System;
using System.Linq;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Models;
using Raven.Client;

namespace AmvReporting.Queries
{
    public class ReportResult
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public String Data { get; set; }
        public String JavaScript { get; set; }
        public String Css { get; set; }
    }


    public class ReportResultQuery : IQuery<ReportResult>
    {
        public int Id { get; set; }

        public ReportResultQuery(int id)
        {
            this.Id = id;
        }
    }

    public class ReportResultQueryHandler : IQueryHandler<ReportResultQuery, ReportResult>
    {
        private readonly IDocumentSession ravenSession;

        public ReportResultQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public ReportResult Handle(ReportResultQuery query)
        {
            var report = ravenSession.Load<ReportDetails>(query.Id);

            throw new NotImplementedException();
        }
    }
}