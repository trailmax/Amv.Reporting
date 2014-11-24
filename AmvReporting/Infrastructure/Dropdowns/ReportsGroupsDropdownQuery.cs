using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;


namespace AmvReporting.Infrastructure.Dropdowns
{
    public class ReportsGroupsDropdownQuery : IDropdownQuery
    {
    }

    public class ReportsGroupsDropdownQueryHandler : IDropdownQueryHandler<ReportsGroupsDropdownQuery>
    {
        private readonly IDocumentSession ravenSession;


        public ReportsGroupsDropdownQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }


        public IEnumerable<SelectListItem> Handle(ReportsGroupsDropdownQuery query)
        {
            var groups = ravenSession.Query<ReportGroup>().ToList();

            var result = groups
                .ToSelectListItems(t => ReportGroupHelpers.GetParentPath(groups, t), v => v.Id)
                .OrderBy(t => t.Text)
                .ToList();

            return result;
        }
    }
}