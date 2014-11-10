using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;
using Raven.Abstractions.Data;
using Raven.Client;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public class MigrationController : BaseController
    {
        private readonly IDocumentSession ravenSession;
        //private readonly IDocumentStore documentStore;
        private readonly IRepository repository;
        private readonly IMediator mediator;


        //public MigrationController(IDocumentSession ravenSession, IRepository repository, IMediator mediator, IDocumentStore documentStore)
        public MigrationController(IDocumentSession ravenSession, IRepository repository, IMediator mediator)
        {
            this.ravenSession = ravenSession;
            this.repository = repository;
            this.mediator = mediator;
            //this.documentStore = documentStore;
        }


        public ActionResult Index()
        {
            var migrationDictionary = new MigrationDictonary();

            var allReports = ravenSession.Query<ReportViewModel>()
                .ToList();

            ViewBag.AllReports = allReports;

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


        public static void RenameCollection(IDocumentStore documentStore)
        {
            documentStore.DatabaseCommands.UpdateByIndex(
                "Raven/DocumentsByEntityName",
                new IndexQuery
                {
                    Query = "Tag:Reports"
                },
                new ScriptedPatchRequest()
                {
                    Script = @"
                                this['@metadata']['Raven-Entity-Name'] = 'ReportViewModels';
                                this['@metadata']['Raven-Clr-Type'] = 'AmvReporting.Domain.Reports.ReportViewModel, AmvReporting';
                                ",
                },
                allowStale: false);
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