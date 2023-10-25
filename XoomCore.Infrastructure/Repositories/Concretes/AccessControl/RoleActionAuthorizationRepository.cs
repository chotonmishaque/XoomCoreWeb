using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;

namespace XoomCore.Infrastructure.Repositories.Concretes.AccessControl;

public class RoleActionAuthorizationRepository : Repository<RoleActionAuthorization>, IRoleActionAuthorizationRepository
{
    public RoleActionAuthorizationRepository(ApplicationDbContext context) : base(context)
    {

    }
}