using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;
using Raven.Client;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public class MigrationController : BaseController
    {
        private readonly IDocumentSession ravenSession;
        private readonly IRepository repository;
        private readonly IMediator mediator;


        public MigrationController(IDocumentSession ravenSession, IRepository repository, IMediator mediator)
        {
            this.ravenSession = ravenSession;
            this.repository = repository;
            this.mediator = mediator;
        }


        public ActionResult Index()
        {
            //var migrationDictionary = new MigrationDictonary();

            //var allReports = ravenSession.Query<Report>()
            //    .ToList();

            //foreach (var oldReport in allReports)
            //{
            //    var newId = Guid.NewGuid();
            //    var oldId = oldReport.Id;
                
            //    var newReport = new Report(newId, oldReport);
            //    var commitId = Guid.NewGuid();
            //    repository.Save(newReport, commitId);

            //    migrationDictionary.Add(oldId, newId);
            //}

            return View();
        }
    }


    public class MigrationDictonary : Dictionary<String, Guid>
    {
        public String Id 
        {
            get
            {
                return "migration/dictionary";
            } 
        }
    }
}