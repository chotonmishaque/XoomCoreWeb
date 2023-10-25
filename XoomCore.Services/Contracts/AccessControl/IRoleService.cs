using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Services.Contracts.AccessControl;

public interface IRoleService
{
    Task<CommonDataTableResponse<IEnumerable<RoleVM>>> GetRoleListAsync(GetDataTableRequest getDataTableRequest);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetRoleListForSelectAsync();
    Task<CommonResponse<RoleVM>> GetRoleAsync(long id);
    Task<CommonResponse<long>> AddRoleAsync(SaveRoleRequest postRoleRequest);
    Task<CommonResponse<long>> EditRoleAsync(SaveRoleRequest putRoleRequest);
    Task<CommonResponse<long>> DeleteRoleAsync(long id);
}
