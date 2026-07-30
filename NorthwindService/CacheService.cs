using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindService
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public bool GetCachedValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void RemoveCache(string key)
        {
            _cache.Remove(key);
        }

        public void SetCache<T>(string key, T value)
        {
            var option = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            _cache.Set(key, value, option);
        }
    }
}
