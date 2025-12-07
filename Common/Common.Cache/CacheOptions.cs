using System;

namespace Common.Cache
{
    public class CacheOptions
    {
        public static int ExpireSlidingCacheFromMinutes => 5;
        public static int FullyExpireCacheFromHour => 24;
    }
}
