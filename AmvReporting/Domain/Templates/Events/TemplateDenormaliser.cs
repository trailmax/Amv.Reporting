using System;
using System.Linq;
using AmvReporting.Infrastructure.Events;
using Omu.ValueInjecter;
using Raven.Client;


namespace AmvReporting.Domain.Templates.Events
{
    public class TemplateDenormaliser : IEventHandler<CreateTemplateEvent>, IEventHandler<UpdateTemplateEvent>
    {
        private readonly IDocumentSession documentSession;


        public TemplateDenormaliser(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }


        public void Handle(CreateTemplateEvent raisedEvent)
        {
            var viewModel = new TemplateViewModel();
            viewModel.InjectFrom(raisedEvent);

            documentSession.Store(viewModel);
            documentSession.SaveChanges();
        }


        public void Handle(UpdateTemplateEvent raisedEvent)
        {
            var viewModel = documentSession.Query<TemplateViewModel>().FirstOrDefault(t => t.AggregateId == raisedEvent.AggregateId);
            viewModel.InjectFrom(raisedEvent);
            documentSession.SaveChanges(); 
        }
    }


    public class TemplateViewModel
    {
        public Guid AggregateId { get; set; }

        public String Title { get; set; }

        public String JavaScript { get; set; }

        public String Html { get; set; }
    }
}