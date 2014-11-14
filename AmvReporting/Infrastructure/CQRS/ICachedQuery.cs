using System;


namespace AmvReporting.Infrastructure.CQRS
{
    public interface ICachedQuery
    {
        String CacheKey { get; }
        int CacheDurationMinutes { get; }
    }
}