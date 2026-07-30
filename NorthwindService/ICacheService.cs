using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindService
{
    public interface ICacheService
    {
        void SetCache<T>(string key, T value);
        void RemoveCache(string key);
        bool GetCachedValue<T>(string key, out T value);
    }
}
