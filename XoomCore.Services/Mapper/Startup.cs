using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Services.Mapper;

internal static class Startup
{
    internal static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AccessControlMappingProfile));
        services.AddAutoMapper(typeof(ReportMappingProfile));
        return services;
    }
}