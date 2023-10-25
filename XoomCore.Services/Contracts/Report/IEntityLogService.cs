using XoomCore.Application.RequestModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.Report;

namespace XoomCore.Services.Contracts.Report;

public interface IEntityLogService
{
    Task<CommonDataTableResponse<IEnumerable<EntityLogVM>>> GetEntityLogListAsync(GetDataTableRequest getDataTableRequest);
}
