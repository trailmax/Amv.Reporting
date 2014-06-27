using System;
using System.Web;
using System.Web.Caching;


namespace AmvReporting.Infrastructure.CQRS
{
    public class CachedDecoratorMediator : IMediator
    {
        private readonly IMediator decorated;


        public CachedDecoratorMediator(IMediator decorated)
        {
            this.decorated = decorated;
        }


        public TResponse Request<TResponse>(IQuery<TResponse> query)
        {
            var cachedQuery = query as ICachedQuery;

            if (cachedQuery == null)
            {
                return decorated.Request(query);
            }


            var cachedEntity = (TResponse)HttpContext.Current.Cache.Get(cachedQuery.CacheKey);
            if (cachedEntity == null)
            {
                cachedEntity = decorated.Request(query);
                HttpContext.Current.Cache.Add(cachedQuery.CacheKey, cachedEntity, 
                    null, Cache.NoAbsoluteExpiration, 
                    TimeSpan.FromMinutes(cachedQuery.CacheDurationMinutes), CacheItemPriority.Normal, null);
            }

            return cachedEntity;
        }
    }
}