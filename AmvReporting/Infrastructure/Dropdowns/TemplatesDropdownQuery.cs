using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.Templates.Events;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;


namespace AmvReporting.Infrastructure.Dropdowns
{
    public class TemplatesDropdownQuery : IDropdownQuery
    {
    }

    public class TemplatesDropdownQueryHandler : IDropdownQueryHandler<TemplatesDropdownQuery>
    {
        private readonly IDocumentSession documentSession;


        public TemplatesDropdownQueryHandler(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }


        public IEnumerable<SelectListItem> Handle(TemplatesDropdownQuery query)
        {
            var templates = documentSession.Query<TemplateViewModel>().OrderBy(t => t.Title)
                .ToSelectListItems(t => t.Title, v => v.AggregateId);

            return templates;
        }
    }
}