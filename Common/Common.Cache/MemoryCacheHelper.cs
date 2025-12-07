using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Common.Cache
{
    public class MemoryCacheHelper : IMemoryCacheHelper
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrSet<T>(string key, Func<Task<T>> func)
        {
            return await _memoryCache.GetOrCreateAsync(key, async (entry) =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(CacheOptions.ExpireSlidingCacheFromMinutes);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CacheOptions.FullyExpireCacheFromHour);
                var res = await func();
                return res;
            });
        }

        public void RemoveCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}