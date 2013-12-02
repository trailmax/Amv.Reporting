using System;
using AmvReporting.Infrastructure.CQRS;

namespace AmvReporting.Queries
{
    public class PreviewDataQuery : IQuery<string>
    {
        public string Sql { get; set; }

        public PreviewDataQuery(String sql)
        {
            Sql = sql;
        }
    }

    public class PreviewDataQueryHandler : IQueryHandler<PreviewDataQuery, String>
    {
        public string Handle(PreviewDataQuery query)
        {
            throw new NotImplementedException();
        }
    }
}