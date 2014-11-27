using System;
using System.Reflection;
using System.Web;
using AmvReporting.Infrastructure.CQRS;
using CommonDomain.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace AmvReporting.Domain.Reports.Queries
{
    public class CompareToLatestViewModel
    {
        public Guid AggregateId { get; set; }

        public String LatestJson { get; set; }

        public String RevisionJson { get; set; }

        public int RevisionNumber { get; set; }
    }

    public class CompareToLatestQuery : IQuery<CompareToLatestViewModel>
    {
        public CompareToLatestQuery(Guid aggregateId, int revisionNumber)
        {
            AggregateId = aggregateId;
            RevisionNumber = revisionNumber;
        }


        public Guid AggregateId { get; private set; }
        public int RevisionNumber { get; private set; }
    }


    public class CompareToLatestQueryHandler : IQueryHandler<CompareToLatestQuery, CompareToLatestViewModel>
    {
        private readonly IRepository repository;

        public CompareToLatestQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }


        public CompareToLatestViewModel Handle(CompareToLatestQuery query)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new TypeOnlyContractResolver<ReportAggregate>(),
            };

            var latest = repository.GetById<ReportAggregate>(query.AggregateId);
            var latestJson = JsonConvert.SerializeObject(latest, Formatting.Indented, serializerSettings);

            var revision = repository.GetById<ReportAggregate>(query.AggregateId, query.RevisionNumber);
            var revisionJson = JsonConvert.SerializeObject(revision, Formatting.Indented, serializerSettings);



            var viewmodel = new CompareToLatestViewModel()
            {
                AggregateId = query.AggregateId,
                RevisionJson = UnescapeString(revisionJson),
                LatestJson = UnescapeString(latestJson),
                RevisionNumber = query.RevisionNumber,
            };

            return viewmodel;
        }

        public static String UnescapeString(String source)
        {
            var unescaped = System.Text.RegularExpressions.Regex.Unescape(source);
            var htmlDecode = HttpUtility.HtmlDecode(unescaped);

            return htmlDecode;
        }
    }


    public class TypeOnlyContractResolver<T> : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            property.ShouldSerialize = instance => property.DeclaringType == typeof(T);
            return property;
        }
    }
}