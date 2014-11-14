using System;
using System.Runtime.Caching;


namespace AmvReporting.Infrastructure.Caching
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private static ObjectCache Cache { get { return MemoryCache.Default; } }

        public object Get(string key)
        {
            return Cache[key];
        }


        public TResult Get<TResult>(string key) where TResult : class
        {
            var result = Get(key) as TResult;

            return result;
        }


        public void Set(string key, object data, int cacheTimeMinutes)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTimeMinutes) };

            Cache.Add(new CacheItem(key, data), policy);
        }


        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }


        public void InvalidateAll()
        {
            foreach (var element in Cache)
            {
                Cache.Remove(element.Key);
            }
        }
    }
}