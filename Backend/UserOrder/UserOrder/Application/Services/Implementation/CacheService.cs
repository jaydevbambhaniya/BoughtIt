using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Services.Interfaces;

namespace UserOrder.Application.Services.Implementation
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _redisCache;
        private readonly IConnectionMultiplexer _redisConnection;
        public CacheService(IConnectionMultiplexer connectionMultiplexer) {
            _redisCache = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> Delete(string key)
        {
            return await _redisCache.KeyDeleteAsync(key);
        }

        public async Task<T> Get<T>(string key)
        {
            var redisValue = await _redisCache.StringGetAsync(key);
            if (redisValue.HasValue)
            {
                var response = JsonConvert.DeserializeObject<T>(redisValue);
                return response;
            }
            return default;
        }

        public async Task<bool> Set<T>(string key, T value,int expiryMinutes)
        {
            var serializedData = JsonConvert.SerializeObject(value);
            return await _redisCache.StringSetAsync(key, serializedData, TimeSpan.FromMinutes(expiryMinutes));
        }
    }
}
