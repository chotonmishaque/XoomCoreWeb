using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Infrastructure.Helpers;


internal static class Startup
{
    internal static IServiceCollection AddSeedData(this IServiceCollection services) =>
            services.AddScoped<SeedDataService>();
}

