using Microsoft.Extensions.Caching.Memory;

namespace XoomCore.Infrastructure.Caching;

public class MemoryCacheManager : ICacheManager
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        return _memoryCache.TryGetValue(key, out T value) ? value : default(T);
    }

    public async Task SetAsync<T>(string key, T value, int expirationMinutes)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
        };

        _memoryCache.Set(key, value, cacheEntryOptions);
    }
}