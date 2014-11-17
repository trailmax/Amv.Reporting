using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmvReporting.Infrastructure.Events;
using Raven.Client;


namespace AmvReporting.Domain.Reports.Events
{
    public class ReportDenormaliser : IEventHandler<ChangeReportListOrderEvent>,
                                      IEventHandler<DisableReportEvent>,
                                      IEventHandler<EnableReportEvent>,
                                      IEventHandler<ReportCodeUpdatedEvent>
    {
        private readonly IDocumentSession documentSession;


        public ReportDenormaliser(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }


        private ReportViewModel GetViewModel(IEvent raisedEvent)
        {
            var viewModel = documentSession.Query<ReportViewModel>().FirstOrDefault(r => r.AggregateId == raisedEvent.AggregateId);
            if (viewModel == null)
            {
                // todo create new view model
            }
            return viewModel;
        }


        public void Handle(ChangeReportListOrderEvent raisedEvent)
        {
            var viewModel = GetViewModel(raisedEvent);
            viewModel.ListOrder = raisedEvent.ListOrder;
            documentSession.SaveChanges();
        }


        public void Handle(DisableReportEvent raisedEvent)
        {
            var viewModel = GetViewModel(raisedEvent);
            viewModel.Enabled = false;
            documentSession.SaveChanges();
        }


        public void Handle(EnableReportEvent raisedEvent)
        {
            var viewModel = GetViewModel(raisedEvent);
            viewModel.Enabled = true;
            documentSession.SaveChanges();
        }


        public void Handle(ReportCodeUpdatedEvent raisedEvent)
        {
            throw new NotImplementedException();
        }
    }
}