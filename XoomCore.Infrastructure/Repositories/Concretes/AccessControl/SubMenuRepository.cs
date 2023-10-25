using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;

namespace XoomCore.Infrastructure.Repositories.Concretes.AccessControl;

public class SubMenuRepository : Repository<SubMenu>, ISubMenuRepository
{
    public SubMenuRepository(ApplicationDbContext context) : base(context)
    {

    }
}
