using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;

namespace XoomCore.Infrastructure.Repositories.Concretes.AccessControl;

public class ActionAuthorizationRepository : Repository<ActionAuthorization>, IActionAuthorizationRepository
{
    public ActionAuthorizationRepository(ApplicationDbContext context) : base(context)
    {

    }
}