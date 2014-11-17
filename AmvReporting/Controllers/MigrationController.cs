using System;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.Migrations;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Indexes;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class MigrationController : BaseController
    {
        private readonly IDocumentSession ravenSession;
        private readonly IDocumentStore documentStore;
        private readonly IRepository repository;
        private readonly IMediator mediator;

        public MigrationController(IDocumentSession ravenSession, IRepository repository, IDocumentStore documentStore, IMediator mediator)
        {
            this.ravenSession = ravenSession;
            this.repository = repository;
            this.documentStore = documentStore;
            this.mediator = mediator;
        }


        public virtual ActionResult Index()
        {
            new RavenDocumentsByEntityName().Execute(documentStore);

            var allViewModelsCount = ravenSession.Query<ReportViewModel>().Count();

            var oldReports = documentStore.DatabaseCommands.Query(
                "Raven/DocumentsByEntityName",
                new IndexQuery
                {
                    Query = "Tag:[[Reports]]",
                }, null);

            var migrationDictionary = mediator.Request(new EventStoreMigrationDictionaryQuery());

            var model = new IndexViewModel()
            {
                ReportViewModelsCount = allViewModelsCount,
                OldReportsCount = oldReports.TotalResults,
                MigrationRecordsCount = migrationDictionary.Count(),
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult RunMigration()
        {
            RenameCollection();

            var allViewModels = ravenSession.Query<ReportViewModel>().ToList();
            var migrationDictionary = mediator.Request(new EventStoreMigrationDictionaryQuery());

            foreach (var oldReport in allViewModels)
            {
                var newId = Guid.NewGuid();
                var oldId = oldReport.Id;
                oldReport.AggregateId = newId;

                var newReport = new ReportAggregate(newId, oldReport);
                var commitId = Guid.NewGuid();
                repository.Save(newReport, commitId);

                migrationDictionary.Add(oldId, newId);
            }
            ravenSession.Store(migrationDictionary);
            ravenSession.SaveChanges();

            return RedirectToAction(MVC.Home.Index());
        }


        private void RenameCollection()
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


    public class IndexViewModel
    {
        public int ReportViewModelsCount { get; set; }
        public int OldReportsCount { get; set; }
        public int MigrationRecordsCount { get; set; }
    }
}