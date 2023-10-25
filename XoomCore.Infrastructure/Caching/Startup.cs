using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Infrastructure.Caching;

internal static class Startup
{
    internal static IServiceCollection AddCaching(this IServiceCollection services) =>
        services.AddScoped<ICacheManager, MemoryCacheManager>();
}
