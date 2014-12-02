using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.ReportGroups;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;


namespace AmvReporting.Infrastructure.Dropdowns
{
    public class ReportGroupParentsDropdownQuery : IDropdownQuery
    {
        public ReportGroupParentsDropdownQuery()
        {
            // nothing here
        }

        public ReportGroupParentsDropdownQuery(string groupId)
        {
            GroupId = groupId;
        }


        public String GroupId { get; private set; }
    }

    public class ReportGroupParentsDropdownQueryHandler : IDropdownQueryHandler<ReportGroupParentsDropdownQuery>
    {
        private readonly IDocumentSession ravenSession;


        public ReportGroupParentsDropdownQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }


        public IEnumerable<SelectListItem> Handle(ReportGroupParentsDropdownQuery query)
        {
            var allGroups = ravenSession.Query<ReportGroup>()
                .Where(g => g.Id != query.GroupId)
                .ToList();

            var possibleParents = allGroups
                .ToSelectListItems(t => ReportGroupHelpers.GetParentPath(allGroups, t), v => v.Id)
                .OrderBy(t => t.Text)
                .ToList();

            return possibleParents;
        }
    }
}