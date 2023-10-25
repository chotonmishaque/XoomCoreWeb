using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Contracts.AccessControl;

namespace XoomCore.Infrastructure.Repositories.Concretes.AccessControl;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {

    }
}
