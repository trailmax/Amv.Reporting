using System.Collections.Generic;
using System.Web.Mvc;
using AmvReporting.Domain.DatabaseConnections;
using AmvReporting.Infrastructure.Helpers;
using Raven.Client;


namespace AmvReporting.Infrastructure.Dropdowns
{
    public class DatabaseDropdownQuery : IDropdownQuery
    {
    }

    public class DatabaseDropdownQueryHandler : IDropdownQueryHandler<DatabaseDropdownQuery>
    {
        private readonly IDocumentSession ravenSession;


        public DatabaseDropdownQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }


        public IEnumerable<SelectListItem> Handle(DatabaseDropdownQuery query)
        {
            var result = ravenSession.Query<DatabaseConnection>()
                .ToSelectListItems(t => t.Name, v => v.Name);

            return result;
        }
    }
}