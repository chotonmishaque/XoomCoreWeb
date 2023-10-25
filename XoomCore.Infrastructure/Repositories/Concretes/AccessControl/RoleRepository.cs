using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;

namespace XoomCore.Infrastructure.Repositories.Concretes.AccessControl;


public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {

    }
}