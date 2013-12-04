using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Queries;

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
        public virtual ActionResult Data(String sql)
        {
            var query = new PreviewDataQuery(sql);
            var result = mediator.Request(query);
            throw new NotImplementedException();
            //return View();
        }
    }
}