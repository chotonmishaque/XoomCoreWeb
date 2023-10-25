using Microsoft.Extensions.DependencyInjection;
using XoomCore.Infrastructure.Caching;
using XoomCore.Infrastructure.Helpers;
using XoomCore.Infrastructure.Persistence;
using XoomCore.Infrastructure.Repositories;
using XoomCore.Infrastructure.UnitOfWorks;

namespace XoomCore.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructures(this IServiceCollection services)
    {
        return services
            .AddMemoryCache()
            .AddCaching()
            .AddPersistence()
            .AddRepositories()
            .AddUnitOfWork()
            .AddSeedData();
    }
}
