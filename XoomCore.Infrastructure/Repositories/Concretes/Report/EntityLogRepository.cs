using XoomCore.Core.Entities.Report;
using XoomCore.Infrastructure.Persistence.Data;
using XoomCore.Infrastructure.Repositories.Contracts.Report;

namespace XoomCore.Infrastructure.Repositories.Concretes.Report;

public class EntityLogRepository : Repository<EntityLog>, IEntityLogRepository
{
    public EntityLogRepository(ApplicationDbContext context) : base(context)
    {

    }
}