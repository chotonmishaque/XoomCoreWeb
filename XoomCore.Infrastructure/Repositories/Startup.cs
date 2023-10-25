using Microsoft.Extensions.DependencyInjection;
using XoomCore.Infrastructure.Repositories.Concretes;
using XoomCore.Infrastructure.Repositories.Concretes.AccessControl;
using XoomCore.Infrastructure.Repositories.Concretes.Report;
using XoomCore.Infrastructure.Repositories.Contracts;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;
using XoomCore.Infrastructure.Repositories.Contracts.Report;

namespace XoomCore.Infrastructure.Repositories;

internal static class Startup
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Register the corresponding repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<ISubMenuRepository, SubMenuRepository>();
        services.AddScoped<IActionAuthorizationRepository, ActionAuthorizationRepository>();
        services.AddScoped<IRoleActionAuthorizationRepository, RoleActionAuthorizationRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IEntityLogRepository, EntityLogRepository>();

        return services;
    }
}
