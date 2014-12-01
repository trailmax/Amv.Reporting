using System;
using System.Linq;
using System.Web.Mvc;
using AmvReporting.Domain.Migrations;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Templates;
using AmvReporting.Domain.Templates.Events;
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

        private TemplateAggregate table;
        private TemplateAggregate lineChart;
        private TemplateAggregate lineChartWithSelecetion;
        private TemplateAggregate pivotedTable;
        private TemplateAggregate googleGraphs;

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

            CreateTemplates();

            foreach (var oldReport in oldReports)
            {
                var newId = Guid.NewGuid();
                var oldId = oldReport.Id;
                migrationDictionary.Add(oldId, newId);

                Guid? templateId = null;
                var htmlOverride = oldReport.HtmlOverride;
                if (oldReport.ReportType == ReportType.Table && string.IsNullOrEmpty(oldReport.JavaScript) && oldReport.HtmlOverride==@"<table class=""table-report""></table>")
                {
                    // clear table explanation
                    templateId = table.Id;
                    htmlOverride = "";
                }
                else if (oldReport.ReportType == ReportType.Table && oldReport.JavaScript.Contains(".PivotedTable("))
                {
                    // pivoted table - clearly
                    templateId = pivotedTable.Id;
                }
                else if (oldReport.ReportType == ReportType.LineChart)
                {
                    templateId = lineChart.Id;
                }
                else if (oldReport.ReportType == ReportType.LineChartWithSelection)
                {
                    templateId = lineChartWithSelecetion.Id;
                }

                var newReport = new ReportAggregate(newId, oldReport.ReportGroupId, oldReport.Title, oldReport.Description, oldReport.DatabaseId, oldReport.Enabled);
                newReport.UpdateCode(templateId, oldReport.Sql, oldReport.JavaScript, htmlOverride);
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

        public void CreateTemplates()
        {
            if (!TemplateExists(MigratingTemplates.Table.Title))
            {
                table = new TemplateAggregate(Guid.NewGuid(), MigratingTemplates.Table.Title, MigratingTemplates.Table.Js, MigratingTemplates.Table.Html);
                repository.Save(table, Guid.NewGuid());
            }

            if (!TemplateExists(MigratingTemplates.LineChart.Title))
            {
                lineChart = new TemplateAggregate(Guid.NewGuid(), MigratingTemplates.LineChart.Title, "", MigratingTemplates.LineChart.Html);
                repository.Save(lineChart, Guid.NewGuid());
            }

            if (!TemplateExists(MigratingTemplates.LineChartWithSelection.Title))
            {
                lineChartWithSelecetion = new TemplateAggregate(Guid.NewGuid(), MigratingTemplates.LineChartWithSelection.Title, "", MigratingTemplates.LineChartWithSelection.Html);
                repository.Save(lineChartWithSelecetion, Guid.NewGuid());
            }

            if (!TemplateExists(MigratingTemplates.GoogleGraphs.Title))
            {
                googleGraphs = new TemplateAggregate(Guid.NewGuid(), MigratingTemplates.GoogleGraphs.Title, "", MigratingTemplates.GoogleGraphs.Html);
                repository.Save(googleGraphs, Guid.NewGuid());
            }

            if (!TemplateExists(MigratingTemplates.PivotedTable.Title))
            {
                pivotedTable = new TemplateAggregate(Guid.NewGuid(), MigratingTemplates.PivotedTable.Title, MigratingTemplates.PivotedTable.Js, MigratingTemplates.PivotedTable.Html);
                repository.Save(pivotedTable, Guid.NewGuid());
            }
        }


        public bool TemplateExists(String title)
        {
            return ravenSession.Query<TemplateViewModel>().Any(t => t.Title == title);
        }
    }


    public class IndexViewModel
    {
        public int ReportViewModelsCount { get; set; }
        public int OldReportsCount { get; set; }
        public int MigrationRecordsCount { get; set; }
    }

    public static class MigratingTemplates
    {
        public static class GlobalConfiguration
        {
            public const String Js = @"(function(window){

    window.ltrTarget=3.6;
    window.turnoverTarget=10;
    
    window.isNumber = function(n){
        return !isNaN(parseFloat(n)) && isFinite(n);
    };

})(window);

//var ltrTarget=3.6;
//var turnoverTarget=10;

//function isNumber(n){
//    return !isNaN(parseFloat(n)) && isFinite(n);
//};";
        }


        public static class Table
        {
            public const String Title = "Table";
            public const String Js = @"var oDataTable = $('.report').dataTable({
    aaData: data,
    aoColumns: columns,
    iDisplayLength: 50,
    sDom: 'Tlfrtip',
    aaSorting: [],
    oTableTools: {
        sSwfPath: '../Scripts/DataTables/extras/TableTools/swf/copy_csv_xls_pdf.swf',
        aButtons: ['copy', 'pdf', 'xls']
    },
    aLengthMenu: [[20, 50, 100, -1], [20, 50, 100, 'All']],
});";
            public const String Html = @"<table class='report' id='report'></table>";
        }


        public static class LineChart
        {
            public const String Title = "Line Chart";
            public const String Html = @"<div class='graph-container'>
    <div id='report' class='report' style='height: 600px;'></div>
</div>";
        }

        public static class LineChartWithSelection
        {
            public const String Title = "Line Chart With Selection";
            public const String Html = @"<div class='row'>
    <div class='col-md-8 graph-container'>
        <div id='report' class='report' style='height: 600px;'></div>
    </div>
    <div class='col-md-4' id='choices'></div>
</div>";
        }

        public static class PivotedTable
        {
            public const String Title = "Pivoted Table";
            public const String Js = @"$.fn.PivotedTable = function (oOptions) {
    var pivotedData = {};
    var columnsDefinition = {};

    oOptions.data.map(function (row) {
        var colTitle = row[oOptions.colTitleColumnName];
        var rowTitle = row[oOptions.rowTitleColumnName];
        var cellValue = row[oOptions.valueColumnName];

        columnsDefinition[colTitle] = { 'mData': colTitle, 'sTitle': colTitle };

        if (!pivotedData[rowTitle]) {
            pivotedData[rowTitle] = {};
        }

        // add current row 'key'
        pivotedData[rowTitle][oOptions.rowTitleColumnName] = rowTitle;

        // all additional columns to be displayed 
        if (oOptions.extraRowData) {
            for (var i = 0; i < oOptions.extraRowData.length; i++) {
                pivotedData[rowTitle][oOptions.extraRowData[i]] = row[oOptions.extraRowData[i]];
            }
        }

        // actual cell values
        pivotedData[rowTitle][colTitle] = cellValue;
    });


    // transorm object of objects into array of objects
    var dataset = [];
    $.each(pivotedData, function (key, values) {
        dataset.push(values);
    });


    // fill in the blanks
    for (var row in dataset) {
        for (var column in columnsDefinition) {
            if (!dataset[row][column]) {
                dataset[row][column] = [];
            }
        }
    }


    // transform columns definition into array of objects
    var columns = [];
    columns.push({ 'mData': oOptions.rowTitleColumnName, 'sTitle': oOptions.rowTitleColumnName });
    if (oOptions.extraRowData) {
        for (var i = 0; i < oOptions.extraRowData.length; i++) {
            columns.push({ 'mData': oOptions.extraRowData[i], 'sTitle': oOptions.extraRowData[i] });
        }
    }

    $.each(columnsDefinition, function (key, values) {
        columns.push(values);
    });


    var dataTableOptions = {
        aaData: dataset,
        aoColumns: columns,
                    iDisplayLength: 20,
                    sDom: 'Tlfrtip',
                    oTableTools: {
                        sSwfPath: '../Scripts/DataTables/extras/TableTools/swf/copy_csv_xls_pdf.swf',
                        aButtons: ['copy', 'pdf', 'xls']
                    },
                    aLengthMenu: [[20, 50, 100, -1],
                                  [20, 50, 100, 'All']],
    };

    if (oOptions.hasOwnProperty('redLimit')) {
        var limit = oOptions.redLimit;

        var highlightFunction = function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).children().each(function (index, td) {
                var value = $(td).html();

                if (isNumber(value) && parseFloat(value) > limit) {
                    $(td).css('background-color', '#FF033E');
                }
                else if (isNumber(value)) {
                    $(td).css('background-color', '#A4C639');
                }
            })
        };

        dataTableOptions.fnRowCallback = highlightFunction;
    }
    
    if (oOptions.hasOwnProperty('aaSorting')) {
        dataTableOptions.aaSorting = oOptions.aaSorting;
    }


     var oTable = this.dataTable(dataTableOptions);

     new FixedHeader(oTable);
};";
            public const String Html = @"<table class='report' id='report'></table>";
        }

        public static class GoogleGraphs
        {
            public const String Title = "Google Graphs";
            public const String Html = @"<script type='text/javascript' src='https://www.google.com/jsapi'></script>
<script>
    // load google chart api
    google.load('visualization', '1.0', { 'packages': ['corechart'] });
</script>
<div class='graph-container'>
    <div id='report' style='height: 600px;'></div>
</div>";
        }
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

    [Obsolete("Replaced by Report Templates")]
    public enum ReportType
    {
        Table,
        LineChart,
        LineChartWithSelection,
        GoogleGraphs,
    }
}