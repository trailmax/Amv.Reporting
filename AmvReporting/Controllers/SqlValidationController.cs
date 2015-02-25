using System;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.Reports.Queries;
using AmvReporting.Domain.SchemaValidation;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Controllers
{
    public partial class SqlValidationController : Controller
    {
        private readonly IMediator mediator;

        public SqlValidationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual ActionResult Index()
        {
            var reports = mediator.Request(new AllReportsQuery()).OrderBy(r => r.Title);

            return View(reports);
        }


        [HttpPost]
        public virtual ActionResult CheckReport(Guid aggregateId)
        {
            var validationResult = mediator.Request(new SqlValidationQuery() { AggregateId = aggregateId });

            return Json(validationResult);
        }
    }
}