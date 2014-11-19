using System.Linq;
using AmvReporting.Infrastructure.Events;
using Omu.ValueInjecter;
using Raven.Client;


namespace AmvReporting.Domain.Reports.Events
{
    public class ReportDenormaliser :
                                    IEventHandler<ChangeReportListOrderEvent>,
                                    IEventHandler<SetReportEnabledEvent>,
                                    IEventHandler<ReportCodeUpdatedEvent>,
                                    IEventHandler<ReportCreatedEvent>,
                                    IEventHandler<UpdateReportMetadaEvent>,
                                    IEventHandler<MigrationEvent>
    {
        private readonly IDocumentSession documentSession;

        public ReportDenormaliser(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }


        private ReportViewModel GetViewModel(IEvent raisedEvent)
        {
            var viewModel = documentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == raisedEvent.AggregateId);

            viewModel = viewModel ?? new ReportViewModel() { AggregateId = raisedEvent.AggregateId };
            return viewModel;
        }


        public void Handle(ChangeReportListOrderEvent raisedEvent)
        {
            var viewmodel = GetViewModel(raisedEvent);
            viewmodel.ListOrder = raisedEvent.ListOrder;
            documentSession.SaveChanges();
        }



        public void Handle(SetReportEnabledEvent raisedEvent)
        {
            var viewmodel = GetViewModel(raisedEvent);
            viewmodel.Enabled = raisedEvent.IsEnabled;
            documentSession.SaveChanges();

        }


        public void Handle(ReportCodeUpdatedEvent raisedEvent)
        {
            var viewmodel = GetViewModel(raisedEvent);
            viewmodel.InjectFrom(raisedEvent);
            documentSession.SaveChanges();
        }


        public void Handle(ReportCreatedEvent raisedEvent)
        {
            var viewmodel = GetViewModel(raisedEvent);
            viewmodel.InjectFrom(raisedEvent);
            documentSession.Store(viewmodel);
            documentSession.SaveChanges();
        }


        public void Handle(UpdateReportMetadaEvent raisedEvent)
        {
            var viewmodel = GetViewModel(raisedEvent);
            viewmodel.InjectFrom(raisedEvent);
            documentSession.SaveChanges();
        }


        public void Handle(MigrationEvent raisedEvent)
        {
            var viewmodel = GetViewModel(raisedEvent);
            viewmodel.InjectFrom(raisedEvent.MigratedReport);
            viewmodel.AggregateId = raisedEvent.AggregateId;
            documentSession.Store(viewmodel);
            documentSession.SaveChanges();

            var viewModel = documentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == raisedEvent.AggregateId);
        }
    }
}