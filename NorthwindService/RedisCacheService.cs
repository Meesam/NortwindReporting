using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindService
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public bool GetCachedValue<T>(string key, out T value)
        {
            
           var json = _distributedCache.GetString(key);
            if(string.IsNullOrEmpty(json))
            {
                value = default;
                return false;
            }
          
            value = JsonConvert.DeserializeObject<T>(json);
            return value != null;
        }

        public void RemoveCache(string key)
        {
            _distributedCache.Remove(key);
        }

        public void SetCache<T>(string key, T value)
        {
            var result =  JsonConvert.SerializeObject(value);
            byte[] byteResult = Encoding.UTF8.GetBytes(result);
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            _distributedCache.Set(key, byteResult, option);
        }
    }
}
