using System;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.Migrations;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;
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

            //var oldReports = documentStore.DatabaseCommands.Query(
            //    "Raven/DocumentsByEntityName",
            //    new IndexQuery
            //    {
            //        Query = "Tag:[[Reports]]",
            //    }, null);

            var oldReports = ravenSession.Query<Report>().Count();

            var migrationDictionary = mediator.Request(new EventStoreMigrationDictionaryQuery());

            var model = new IndexViewModel()
            {
                ReportViewModelsCount = allViewModelsCount,
                OldReportsCount = oldReports,
                MigrationRecordsCount = migrationDictionary.Count(),
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult RunMigration()
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 2048;

            var migrationDictionary = mediator.Request(new EventStoreMigrationDictionaryQuery());

            var migratedDocumentsIds = migrationDictionary.Keys.ToList();
            var oldReports = ravenSession.Query<Report>().ToList().Where(r => !migratedDocumentsIds.Contains(r.Id)).ToList();

            foreach (var oldReport in oldReports)
            {
                var newId = Guid.NewGuid();
                var oldId = oldReport.Id;
                migrationDictionary.Add(oldId, newId);

                var newReport = new ReportAggregate(newId, oldReport.ReportGroupId, oldReport.Title, oldReport.ReportType, oldReport.Description, oldReport.DatabaseId, oldReport.Enabled);
                newReport.UpdateCode(oldReport.Sql, oldReport.JavaScript, oldReport.Css, oldReport.HtmlOverride);
                newReport.SetListOrder(oldReport.ListOrder ?? 0);

                repository.Save(newReport, Guid.NewGuid());
            }
            ravenSession.Store(migrationDictionary);
            ravenSession.SaveChanges();

            return RedirectToAction(MVC.Migration.Index());
        }


//        private void RenameCollection()
//        {
//            documentStore.DatabaseCommands.UpdateByIndex(
//                "Raven/DocumentsByEntityName",
//                new IndexQuery
//                {
//                    Query = "Tag:Reports"
//                },
//                new ScriptedPatchRequest()
//                {
//                    Script = @"
//                                this['@metadata']['Raven-Entity-Name'] = 'ReportViewModels';
//                                this['@metadata']['Raven-Clr-Type'] = 'AmvReporting.Domain.Reports.ReportViewModel, AmvReporting';
//                                ",
//                },
//                allowStale: false);
//        }
    }


    public class IndexViewModel
    {
        public int ReportViewModelsCount { get; set; }
        public int OldReportsCount { get; set; }
        public int MigrationRecordsCount { get; set; }
    }
}

namespace AmvReporting.Domain.Reports
{
    [Obsolete("This is the old model required only to do a migration. Don't remove the namespace or class name")]
    public class Report
    {
        public String Id { get; set; }

        public String ReportGroupId { get; set; }

        public String Title { get; set; }

        public ReportType ReportType { get; set; }

        public String Description { get; set; }

        public String Sql { get; set; }

        public String JavaScript { get; set; }

        public String Css { get; set; }

        [AllowHtml]
        public String HtmlOverride { get; set; }

        public String DatabaseId { get; set; }

        public bool Enabled { get; set; }

        public int? ListOrder { get; set; }
    }
}