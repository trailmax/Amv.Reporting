using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AmvReporting.Domain.Reports;
using AmvReporting.Domain.Search;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace AmvReporting.Infrastructure.RavenIndexes
{
    public class SearchIndex : AbstractIndexCreationTask<ReportViewModel>
    {
        public SearchIndex()
        {
            Map = row => row.Select(c => new SearchResult()
            {
                Sql = c.Sql,
                Title = c.Title,
            });

            Indexes = new Dictionary<Expression<Func<ReportViewModel, object>>, FieldIndexing>()
            {
                { r => r.Sql, FieldIndexing.Analyzed }, 
                { r => r.Title, FieldIndexing.Analyzed },
            };

            Analyzers = new Dictionary<Expression<Func<ReportViewModel, object>>, string>()
            {
                { r => r.Sql, "SimpleAnalyzer"},
                { r => r.Title, "SimpleAnalyzer" },
            };
        }
    }
}