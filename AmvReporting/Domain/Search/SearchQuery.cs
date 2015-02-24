using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.RavenIndexes;
using AutoMapper;
using Raven.Client;


namespace AmvReporting.Domain.Search
{
    public class SearchQuery : IQuery<IEnumerable<SearchResult>>
    {
        [Required]
        public String SearchTerms { get; set; }
    }

    public class SearchResult
    {
        public Guid AggregateId { get; set; }

        public String Title { get; set; }

        public String Sql { get; set; }

        public String Description { get; set; }
    }

    public class SearchQueryHandler : IQueryHandler<SearchQuery, IEnumerable<SearchResult>>
    {
        private readonly IDocumentSession ravenSession;

        public SearchQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public IEnumerable<SearchResult> Handle(SearchQuery query)
        {
            var report = ravenSession.Query<ReportViewModel, SearchIndex>()
                .Search(r => r.Sql, query.SearchTerms, boost: 10, escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                .Search(r => r.Title, query.SearchTerms, boost: 5, escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                .Take(20)
                .ToList();

            var result = Mapper.Map<IEnumerable<SearchResult>>(report);

            return result;
        }
    }
}