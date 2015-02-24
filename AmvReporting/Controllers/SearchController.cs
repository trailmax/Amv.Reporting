using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.Search;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;

namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class SearchController : BaseController
    {
        private readonly IMediator mediator;

        public SearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public virtual ActionResult Index(SearchQuery query)
        {
            var searchResults = mediator.Request(query);

            var model = new SearchPageViewModel()
            {
                SearchTerms = query.SearchTerms,
                SearchResults = searchResults.ToList(),
            };


            return View(model);
        }
    }

    public class SearchPageViewModel
    {
        [Required]
        public String SearchTerms { get; set; }


        public List<SearchResult> SearchResults { get; set; }
    }
}