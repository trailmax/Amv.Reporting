using System;
using System.Web.Mvc;
using AmvReporting.Domain.Preview;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    public partial class PreviewController : BaseController
    {
        private readonly IMediator mediator;

        public PreviewController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public virtual ActionResult Data(String sql, String databaseId)
        {
            var query = new PreviewDataQuery(sql, databaseId);
            var result = mediator.Request(query);

            return PartialView(result);
        }
    }
}