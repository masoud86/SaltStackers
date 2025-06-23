using SaltStackers.Application.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SaltStackers.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _redisCache;
        public static readonly string RedisConnectionString = "localhost:6379,ssl=False,abortConnect=False";
        public CacheService()
        {
            _redisCache = GetConnection().GetDatabase();
        }

        private static ConnectionMultiplexer GetConnection()
        {
            string connectionString = RedisConnectionString;
            return ConnectionMultiplexer.Connect(connectionString);
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _redisCache.StringSetAsync(key, value, expiry);
        }

        public async Task SetAsync(string key, object value, TimeSpan? expiry = null)
        {
            await _redisCache.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
        }

        public async Task<string> GetAsync(string key)
        {
            return await _redisCache.StringGetAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var value = await _redisCache.StringGetAsync(key);
                if (!value.IsNull)
                {
                    return JsonSerializer.Deserialize<T>(value);
                }

                return default;
            }
            catch
            {
                return default;
            }
        }

        public async Task<string> GetOrSetAsync(string key, string value, TimeSpan? expiry = null)
        {
            var result = await _redisCache.StringGetAsync(key);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            await SetAsync(key, value, expiry);
            return value;
        }

        public async Task<T> GetOrSetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var result = await GetAsync<T>(key);
            if (result != null)
            {
                return result;
            }

            await SetAsync(key, value, expiry);
            return value;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<T> builder, TimeSpan? expiry = null)
        {
            var result = await GetAsync<T>(key);
            if (result != null)
            {
                return result;
            }

            result = builder();
            await SetAsync(key, result, expiry);
            return result;
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            return await _redisCache.KeyExistsAsync(key);
        }
    }
}
