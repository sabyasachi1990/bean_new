using System;

namespace WebApi.RedisCache.Core.Time
{
    public class CacheTime
    {
        // client cache length in seconds
        public TimeSpan ClientTimeSpan { get; set; }

        public DateTimeOffset AbsoluteExpiration { get; set; }
    }
}