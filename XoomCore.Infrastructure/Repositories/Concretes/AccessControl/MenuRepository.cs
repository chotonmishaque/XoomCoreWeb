using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;

namespace XoomCore.Infrastructure.Repositories.Concretes.AccessControl;


public class MenuRepository : Repository<Menu>, IMenuRepository
{
    public MenuRepository(ApplicationDbContext context) : base(context)
    {

    }
}
