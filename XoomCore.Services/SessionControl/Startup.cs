using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Services.SessionControl;

internal static class Startup
{
    internal static IServiceCollection AddSessionControl(this IServiceCollection services) =>
        services.AddScoped<ISessionManager, SessionManager>();
}
