using System.Collections.Generic;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;


namespace AmvReporting.Infrastructure.Dropdowns
{
    public interface IDropdownQueryHandler<in TQuery> : IQueryHandler<TQuery, IEnumerable<SelectListItem>> where TQuery : IDropdownQuery { }
}