using System;


namespace AmvReporting.Infrastructure.CQRS
{
    public interface IQuery<TResult>
    {
    }


    public interface ICachedQuery
    {
        String CacheKey { get; }
        int CacheDurationMinutes { get; }
    }
}