using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Filters;
using CommonDomain.Persistence;
using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database;


namespace AmvReporting.Controllers
{
    [RoleAuthorizeFilter]
    public partial class MigrationController : BaseController
    {
        private readonly IDocumentSession ravenSession;
        private readonly IDocumentStore documentStore;
        private readonly IRepository repository;
        private readonly IMediator mediator;


        public MigrationController(IDocumentSession ravenSession, IRepository repository, IMediator mediator, IDocumentStore documentStore)
        {
            this.ravenSession = ravenSession;
            this.repository = repository;
            this.mediator = mediator;
            this.documentStore = documentStore;
        }


        public virtual ActionResult Index()
        {
            var allViewModels = ravenSession.Query<ReportViewModel>().ToList();
            new RavenDocumentsByEntityName().Execute(documentStore);

            var oldReports = documentStore.DatabaseCommands.Query(
                "Raven/DocumentsByEntityName",
                new IndexQuery
                {
                    Query = "Tag:[[Reports]]",
                }, null);

            var model = new IndexViewModel()
            {
                ReportViewModels = allViewModels,
                OldReportsCount = oldReports.TotalResults,
            };

            return View(model);
        }





        [HttpPost]
        public virtual ActionResult RunMigration()
        {
            throw new NotImplementedException();
            RenameCollection();

            var allViewModels = ravenSession.Query<ReportViewModel>().ToList();
            var migrationDictionary = new MigrationDictonary();

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
        public List<ReportViewModel> ReportViewModels { get; set; }
        public int OldReportsCount { get; set; }
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