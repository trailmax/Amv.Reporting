namespace AmvReporting.Infrastructure.Caching
{
    public interface ICacheProvider
    {
        object Get(string key);
        TResult Get<TResult>(string key) where TResult : class;
        void Set(string key, object data, int cacheTimeMinutes);
        bool IsSet(string key);
        void Invalidate(string key);
        void InvalidateAll();
    }
}