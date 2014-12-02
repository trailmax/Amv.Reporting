using System.Collections.Generic;
using System.Linq;
using AmvReporting.Domain.Templates.Events;
using AmvReporting.Infrastructure.CQRS;
using Raven.Client;


namespace AmvReporting.Domain.Templates.Queries
{
    public class AllTemplatesQuery : IQuery<IEnumerable<TemplateViewModel>>
    {
    }

    public class AllTemplatesQueryHandler : IQueryHandler<AllTemplatesQuery, IEnumerable<TemplateViewModel>>
    {
        private readonly IDocumentSession documentSession;


        public AllTemplatesQueryHandler(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }


        public IEnumerable<TemplateViewModel> Handle(AllTemplatesQuery query)
        {
            var allTemplates = documentSession.Query<TemplateViewModel>().ToList();
            return allTemplates;
        }
    }
}