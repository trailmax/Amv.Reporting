﻿using System;
using System.Web.Mvc;
using AmvReporting.Domain.Preview.Queries;
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
            var result = mediator.Request(new PreviewDataQuery(aggregateId, sql));

            return PartialView(result);
        }


        [HttpPost]
        public virtual ActionResult Report(PreviewReportQuery query)
        {
            var reportPreview = mediator.Request(query);
            return PartialView(reportPreview);
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