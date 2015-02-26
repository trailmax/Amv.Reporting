using System;
using System.Web.Mvc;
using AmvReporting.Domain.Search;
using AmvReporting.Infrastructure.Filters;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class SearchController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult IndexSubmitted(String searchTerms, int pageNumber = 0)
        {
            var query = new SearchQuery()
            {
                SearchTerms = searchTerms,
                PageNumber = pageNumber,
            };

            return QueriedView(query).ShowView("Index");
        }
    }
}