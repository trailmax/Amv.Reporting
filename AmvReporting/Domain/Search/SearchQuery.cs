using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AmvReporting.Domain.Reports;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.RavenIndexes;
using AutoMapper;
using Raven.Client;
using Raven.Client.Linq;


namespace AmvReporting.Domain.Search
{
    public class SearchQuery : IQuery<PagedSearchResult>
    {
        [Required]
        public String SearchTerms { get; set; }

        public int PageNumber { get; set; }
    }

    public class PagedSearchResult
    {
        public int TotalNumberOfResults { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }

        public String SearchTerms { get; set; }
        public List<SearchResult> Results { get; set; }
    }

    public class SearchResult
    {
        public Guid AggregateId { get; set; }

        public String Title { get; set; }

        public String Sql { get; set; }

        public String Description { get; set; }
    }

    public class SearchQueryHandler : IQueryHandler<SearchQuery, PagedSearchResult>
    {
        private readonly IDocumentSession ravenSession;

        public SearchQueryHandler(IDocumentSession ravenSession)
        {
            this.ravenSession = ravenSession;
        }

        public PagedSearchResult Handle(SearchQuery query)
        {
            var pageSize = 20;


            RavenQueryStatistics stats;
            var reports = ravenSession.Query<ReportViewModel, SearchIndex>()
                .Statistics(out stats)
                .Search(r => r.Sql, query.SearchTerms, boost: 10, escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                .Search(r => r.Title, query.SearchTerms, boost: 5, escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
                .Skip(pageSize * query.PageNumber)
                .Take(pageSize)
                .ToList();

            var searchResults = Mapper.Map<List<SearchResult>>(reports);

            var numberOfPages = stats.TotalResults > 0 ? (int)Math.Ceiling((double)(stats.TotalResults / pageSize)) : 0;
            var nextPage = (query.PageNumber + 1) > numberOfPages ? (int?)null : query.PageNumber + 1;
            var prevPage = (query.PageNumber - 1) < 0 ? (int?) null : query.PageNumber - 1;

            var pagedSearchResult = new PagedSearchResult()
            {
                SearchTerms = query.SearchTerms,
                CurrentPage = query.PageNumber,
                NumberOfPages = numberOfPages,
                TotalNumberOfResults = stats.TotalResults,
                NextPage = nextPage,
                PreviousPage = prevPage,
                Results = searchResults,
            };

            return pagedSearchResult;
        }
    }
}