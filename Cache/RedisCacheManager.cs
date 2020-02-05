using System;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cache
{
    public class RedisCacheManager
    {
        private RedisCacheOptions options;
        public RedisCacheManager()
        {
            options = new RedisCacheOptions
            {
                Configuration = "127.0.0.1:6379",
                InstanceName = ""
            };
        }
        public T Get<T>(string key)
        {
            using (var redisCache = new RedisCache(options))
            {
                var valueString = redisCache.GetString(key);
                if (!string.IsNullOrEmpty(valueString))
                {
                    var valueObject = JsonConvert.DeserializeObject<T>(valueString);
                    return (T)valueObject;
                }

                return default(T);
            }
        }

        public void Set(string key, object valueObject, int expiration)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiration)
            };

            using (var redisCache = new RedisCache(options))
            {
                var valueString = JsonConvert.SerializeObject(valueObject);
                redisCache.SetString(key, valueString, cacheOptions);
            }
        }

        public void Remove(string key)
        {
            using (var redisCache = new RedisCache(options))
            {
                redisCache.Remove(key);
            }
        }

        public void RemoveByPattern(string pattern)
        {
            using (var redisConnection = ConnectionMultiplexer.Connect(options.Configuration))
            {
                var redisServer = redisConnection.GetServer(redisConnection.GetEndPoints().First());
                var redisDatabase = redisConnection.GetDatabase();

                foreach (var key in redisServer.Keys(pattern: pattern))
                {
                    redisDatabase.KeyDelete(key);
                }
            }
        }
    }
}
