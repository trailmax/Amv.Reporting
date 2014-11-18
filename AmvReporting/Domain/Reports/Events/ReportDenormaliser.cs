using System.Linq;
using AmvReporting.Infrastructure.Events;
using AutoMapper;
using CommonDomain.Persistence;
using Raven.Client;


namespace AmvReporting.Domain.Reports.Events
{
    public class ReportDenormaliser :
                                    IEventHandler<ChangeReportListOrderEvent>,
                                    IEventHandler<DisableReportEvent>,
                                    IEventHandler<EnableReportEvent>,
                                    IEventHandler<ReportCodeUpdatedEvent>,
                                    IEventHandler<ReportCreatedEvent>,
                                    IEventHandler<UpdateReportMetadaEvent>
    {
        private readonly IDocumentSession documentSession;
        private readonly IRepository reposity;

        public ReportDenormaliser(IDocumentSession documentSession, IRepository reposity)
        {
            this.documentSession = documentSession;
            this.reposity = reposity;
        }


        private void Denormalise(IEvent raisedEvent)
        {
            var report = reposity.GetById<ReportAggregate>(raisedEvent.AggregateId);

            var viewModel = documentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == raisedEvent.AggregateId);

            viewModel = viewModel ?? new ReportViewModel() { AggregateId = raisedEvent.AggregateId };

            Mapper.Map(report, viewModel);
            //viewModel.ReportGroupId = report.ReportGroupId;
            //viewModel.Title = report.Title;
            //viewModel.ReportType = report.ReportType;
            //viewModel.Description = report.Description;
            //viewModel.Sql = report.Sql;
            //viewModel.JavaScript = report.JavaScript;
            //viewModel.Css = report.Css;
            //viewModel.HtmlOverride = report.HtmlOverride;
            //viewModel.DatabaseId = report.DatabaseId;
            //viewModel.Enabled = report.Enabled;
            //viewModel.ListOrder = report.ListOrder;
            //viewModel.AggregateId = report.Id;

            documentSession.Store(viewModel);
            documentSession.SaveChanges();
        }


        public void Handle(ChangeReportListOrderEvent raisedEvent)
        {
            Denormalise(raisedEvent);
        }


        public void Handle(DisableReportEvent raisedEvent)
        {
            Denormalise(raisedEvent);
        }


        public void Handle(EnableReportEvent raisedEvent)
        {
            Denormalise(raisedEvent);
        }


        public void Handle(ReportCodeUpdatedEvent raisedEvent)
        {
            Denormalise(raisedEvent);
        }


        public void Handle(ReportCreatedEvent raisedEvent)
        {
            Denormalise(raisedEvent);
        }


        public void Handle(UpdateReportMetadaEvent raisedEvent)
        {
            Denormalise(raisedEvent);
        }
    }
}