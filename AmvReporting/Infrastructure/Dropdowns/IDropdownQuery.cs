using System.Collections.Generic;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;


namespace AmvReporting.Infrastructure.Dropdowns
{
    public interface IDropdownQuery : IQuery<IEnumerable<SelectListItem>> { }
}