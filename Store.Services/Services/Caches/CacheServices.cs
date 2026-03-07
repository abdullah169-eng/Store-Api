using StackExchange.Redis;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Store.Services.Services.Caches
{
    public class CacheServices : ICacheServices
    {
        private readonly IDatabase _redis;

        public CacheServices(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }
        public async Task<string> GetCacheKeyAsync(string key)
        {
            var value = await _redis.StringGetAsync(key);
            if (value.IsNullOrEmpty) return null;
            return value.ToString();
        }
        public async Task SetCachekeyAsync(string key, object value, TimeSpan expiration)
        {
            if (value == null) return;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            await _redis.StringSetAsync(key, JsonSerializer.Serialize(value,options), expiration);
        }
    }
}
