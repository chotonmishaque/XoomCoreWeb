using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Services.Contracts.AccessControl;

public interface IUserRoleService
{
    Task<CommonDataTableResponse<IEnumerable<UserRoleVM>>> GetUserRoleListAsync(GetDataTableRequest getDataTableRequest);
    Task<CommonResponse<UserRoleVM>> GetUserRoleAsync(long id);
    Task<CommonResponse<long>> AddUserRoleAsync(SaveUserRoleRequest postUserRoleRequest);
    Task<CommonResponse<long>> EditUserRoleAsync(SaveUserRoleRequest putUserRoleRequest);
    Task<CommonResponse<long>> DeleteUserRoleAsync(long id);

}