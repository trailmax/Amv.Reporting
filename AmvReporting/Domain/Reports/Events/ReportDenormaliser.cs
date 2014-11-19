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
                                    IEventHandler<UpdateReportMetadaEvent>
    {
        private readonly IDocumentSession documentSession;
        private readonly IMediator mediator;


        public ReportDenormaliser(IDocumentSession documentSession, IMediator mediator)
        {
            this.documentSession = documentSession;
            this.mediator = mediator;
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
            var viewmodel = new ReportViewModel() { AggregateId = raisedEvent.AggregateId };
            viewmodel.InjectFrom(raisedEvent);

            var databaseDetails = mediator.Request(new DatabaseQuery(raisedEvent.DatabaseId));
            viewmodel.ConnectionString = databaseDetails.CheckForNull(d => d.ConnectionString);

            documentSession.Store(viewmodel);
            documentSession.SaveChanges();
        }


        public void Handle(UpdateReportMetadaEvent raisedEvent)
        {
            var viewmodel = GetViewModel(raisedEvent);
            viewmodel.InjectFrom(raisedEvent);
            var databaseDetails = mediator.Request(new DatabaseQuery(raisedEvent.DatabaseId));
            viewmodel.ConnectionString = databaseDetails.CheckForNull(d => d.ConnectionString);

            documentSession.SaveChanges();
        }


        private ReportViewModel GetViewModel(IEvent raisedEvent)
        {
            var viewModel = documentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == raisedEvent.AggregateId);

            return viewModel;
        }
    }
}