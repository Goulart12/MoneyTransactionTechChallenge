using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace MoneyTransactionTechChallenge.Helpers.CacheHelpers;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache? _cache;
    
    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public T GetCachedData<T>(string key)
    {
        var jsonData = _cache.GetString(key);
        if (jsonData == null)
            return default(T);
        return JsonSerializer.Deserialize<T>(jsonData);
    }
    
    public void SetCachedData<T>(string key, T data, TimeSpan cacheDuration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration
        };
        var jsonData = JsonSerializer.Serialize(data);
        _cache.SetString(key, jsonData, options);
    }

    public void RemoveCachedData(string key)
    {
        _cache.Remove(key);
    }
}