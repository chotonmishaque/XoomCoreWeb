using Microsoft.Extensions.DependencyInjection;
using XoomCore.Services.Concretes.AccessControl;
using XoomCore.Services.Concretes.Report;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.Contracts.Report;

namespace XoomCore.Services.Contracts;


internal static class Startup
{
    internal static IServiceCollection AddContracts(this IServiceCollection services)
    {
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<ISubMenuService, SubMenuService>();
        services.AddScoped<IActionAuthorizationService, ActionAuthorizationService>();
        services.AddScoped<IRoleActionAuthorizationService, RoleActionAuthorizationService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IEntityLogService, EntityLogService>();
        return services;
    }
}
