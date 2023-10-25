using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Infrastructure.UnitOfWorks;


internal static class Startup
{
    internal static IServiceCollection AddUnitOfWork(this IServiceCollection services) =>
            services.AddScoped<IUnitOfWork, UnitOfWork>();
}

