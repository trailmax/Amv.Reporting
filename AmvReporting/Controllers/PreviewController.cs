using System;
using System.Web.Mvc;
using AmvReporting.Domain.Preview.Queries;
using AmvReporting.Domain.Preview.ViewModels;
using AmvReporting.Domain.ReportingConfigs.Queries;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class PreviewController : BaseController
    {
        private readonly IMediator mediator;

        public PreviewController(IMediator mediator)
        {
            this.mediator = mediator;
        }



        [HttpPost]
        public virtual ActionResult Data(Guid aggregateId, String sql)
        {
            var query = new PreviewDataQuery(aggregateId, sql);
            var result = mediator.Request(query);

            return PartialView(result);
        }


        [HttpPost]
        public virtual ActionResult Report(PreviewReportModel model)
        {
            var query = new PreviewDataQuery(model.AggregateId, model.Sql);
            var queryResult = mediator.Request(query);
            var config = mediator.Request(new ReportingConfigQuery());

            var outModel = new ReportResultPreview()
                           {
                               Data = queryResult.Data,
                               JavaScript = model.JavaScript,
                               GlobalJs = config.GlobalJavascript,
                               Css = model.Css,
                               GlobalCss = config.GlobalCss,
                               ReportType = queryResult.ReportType,
                               HtmlOverride = model.HtmlOverride,
                           };

            return PartialView(outModel);
        }


        [HttpPost, ValidateInput(false)]
        public virtual ActionResult CleanseAndFormatSql(String sql)
        {
            var cleanedSql = mediator.Request(new CleanseSqlQuery(sql));

            var formattedSql = mediator.Request(new FormattedSqlQuery(cleanedSql));

            return Json(formattedSql);
        }
    }
}