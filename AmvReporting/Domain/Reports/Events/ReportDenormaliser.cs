using System.Linq;
using AmvReporting.Domain.DatabaseConnections.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Events;
using AmvReporting.Infrastructure.Helpers;
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
                                    IEventHandler<DeleteReportEvent>
    {
        private readonly IDocumentStore documentStore;
        private readonly IMediator mediator;


        public ReportDenormaliser(IMediator mediator, IDocumentStore documentStore)
        {
            this.mediator = mediator;
            this.documentStore = documentStore;
        }


        public void Handle(ChangeReportListOrderEvent raisedEvent)
        {
            using (var documentSession = documentStore.OpenSession())
            {
                var viewmodel = GetViewModel(raisedEvent, documentSession);
                viewmodel.ListOrder = raisedEvent.ListOrder;
                documentSession.SaveChanges();
            }
        }



        public void Handle(SetReportEnabledEvent raisedEvent)
        {
            using (var documentSession = documentStore.OpenSession())
            {
                var viewmodel = GetViewModel(raisedEvent, documentSession);
                viewmodel.Enabled = raisedEvent.IsEnabled;
                documentSession.SaveChanges();
            }
        }


        public void Handle(ReportCodeUpdatedEvent raisedEvent)
        {
            using (var documentSession = documentStore.OpenSession())
            {
                var viewmodel = GetViewModel(raisedEvent, documentSession);
                viewmodel.InjectFrom(raisedEvent);
                documentSession.SaveChanges();
            }
        }


        public void Handle(ReportCreatedEvent raisedEvent)
        {
            using (var documentSession = documentStore.OpenSession())
            {
                var viewmodel = new ReportViewModel() { AggregateId = raisedEvent.AggregateId };
                viewmodel.InjectFrom(raisedEvent);

                var databaseDetails = mediator.Request(new DatabaseQuery(raisedEvent.DatabaseId));
                viewmodel.ConnectionString = databaseDetails.CheckForNull(d => d.ConnectionString);

                documentSession.Store(viewmodel);
                documentSession.SaveChanges();
            }
        }


        public void Handle(UpdateReportMetadaEvent raisedEvent)
        {
            using (var documentSession = documentStore.OpenSession())
            {
                var viewmodel = GetViewModel(raisedEvent, documentSession);
                viewmodel.InjectFrom(raisedEvent);
                var databaseDetails = mediator.Request(new DatabaseQuery(raisedEvent.DatabaseId));
                viewmodel.ConnectionString = databaseDetails.CheckForNull(d => d.ConnectionString);

                documentSession.SaveChanges();
            }
        }


        private ReportViewModel GetViewModel(IEvent raisedEvent, IDocumentSession documentSession)
        {
            var viewModel = documentSession.Query<ReportViewModel>()
                .FirstOrDefault(r => r.AggregateId == raisedEvent.AggregateId);

            return viewModel;
        }


        public void Handle(DeleteReportEvent raisedEvent)
        {
            using (var documentSession = documentStore.OpenSession())
            {
                var toBeDeleted = GetViewModel(raisedEvent, documentSession);
                documentSession.Delete(toBeDeleted);
                documentSession.SaveChanges();
            }
        }
    }
}