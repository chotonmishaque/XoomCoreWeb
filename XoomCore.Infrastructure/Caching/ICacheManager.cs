namespace XoomCore.Infrastructure.Caching;

public interface ICacheManager
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, int expirationMinutes = 30);
}
