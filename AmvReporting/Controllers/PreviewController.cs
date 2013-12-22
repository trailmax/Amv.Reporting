using System;
using System.Web.Mvc;
using AmvReporting.Domain.Preview.Queries;
using AmvReporting.Domain.Preview.ViewModels;
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
        public virtual ActionResult Data(String sql, String databaseId)
        {
            var query = new PreviewTableQuery(sql, databaseId);
            var result = mediator.Request(query);

            return PartialView(result);
        }


        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Report(PreviewReportModel model)
        {
            var query = new PreviewTableQuery(model.Sql, model.DatabaseId);
            var queryResult = mediator.Request(query);

            var outModel = new ReportResultPreview()
                           {
                               Data = queryResult.Data,
                               Columns = queryResult.Columns,
                               JavaScript = model.JavaScript,
                               Css = model.Css,
                               ReportType = model.ReportType,
                           };

            return PartialView(outModel);
        }


        [HttpPost]
        public virtual ActionResult CleanseAndFormatSql(String sql)
        {
            var cleanedSql = mediator.Request(new CleanseSqlQuery(sql));

            var formattedSql = mediator.Request(new FormattedSqlQuery(cleanedSql));

            return Json(formattedSql);
        }
    }
}