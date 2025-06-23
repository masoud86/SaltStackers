using System;
using System.Threading.Tasks;

namespace SaltStackers.Application.Interfaces
{
    public interface ICacheService
    {
        Task SetAsync(string key, string value, TimeSpan? expiry = null);

        Task SetAsync(string key, object value, TimeSpan? expiry = null);

        Task<string> GetAsync(string key);

        Task<T> GetAsync<T>(string key);

        Task<string> GetOrSetAsync(string key, string value, TimeSpan? expiry = null);

        Task<T> GetOrSetAsync<T>(string key, T value, TimeSpan? expiry = null);

        Task<T> GetOrSetAsync<T>(string key, Func<T> builder, TimeSpan? expiry = null);

        Task<bool> ContainsKeyAsync(string key);
    }
}
