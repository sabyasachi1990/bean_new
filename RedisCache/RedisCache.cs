using System;
using System.Linq;
using System.Configuration;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Infrastructure.Cache.RedisCache
{
    public class RedisCache
    {
        private volatile static RedisCache redisCacheObj;
        private static RedisClient instance;

        private RedisCache()
        {
            instance = new RedisClient(ConfigurationManager.AppSettings["RedisServer"].ToString());
        }

        public static RedisCache CreateInstance()
        {
            if (redisCacheObj == null)
            {
                redisCacheObj = new RedisCache();
            }
            return redisCacheObj;
        }

        public bool CacheExists(string key)
        {
            return (Convert.ToInt16(instance.GetListCount(key)) == 0) ? false : true;
        }

        public IQueryable<T> GetCacheData<T>(string key)
        {
            IRedisTypedClient<T> redisTermsOfPayments = instance.As<T>();

            return redisTermsOfPayments.Lists[key].AsQueryable<T>();
        }

        public void UpdateCahceDate<T>(string key, T data)
        {
            IRedisTypedClient<T> redisTermsOfPayments = instance.As<T>();
            redisTermsOfPayments.Lists[key].RemoveAll();
            redisTermsOfPayments.Lists[key].Add(data);
        }

        public IQueryable<T> GetDataFromDB<T>(string key, T data)
        {
            IRedisTypedClient<T> redisTermsOfPayments = instance.As<T>();

            redisTermsOfPayments.Lists[key].Add(data);

            return redisTermsOfPayments.Lists[key].AsQueryable<T>();
        }


        public void InvalidateCache<T>(string key)
        {
            IRedisTypedClient<T> redisTermsOfPayments = instance.As<T>();

            redisTermsOfPayments.Lists[key].RemoveAll();
        }
    }
}
