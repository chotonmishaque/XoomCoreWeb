using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using XoomCore.Services.Contracts;
using XoomCore.Services.Mapper;
using XoomCore.Services.Middleware;
using XoomCore.Services.SessionControl;

namespace XoomCore.Services;

public static class Startup
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            //.AddExceptionMiddleware()
            .AddMappers()
            .AddSessionControl()
            .AddContracts();
    }

    public static IApplicationBuilder UseServices(this IApplicationBuilder builder) =>
        builder
            .UseExceptionMiddleware();
}
