using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Services.Contracts.AccessControl;

public interface IRoleActionAuthorizationService
{
    Task<CommonDataTableResponse<IEnumerable<RoleActionAuthorizationVM>>> GetRoleActionAuthorizationListAsync(GetDataTableRequest getDataTableRequest);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetRoleActionAuthorizationListForSelectAsync();
    Task<CommonResponse<RoleActionAuthorizationVM>> GetRoleActionAuthorizationAsync(long id);
    Task<CommonResponse<SaveRoleActionAuthorizationResponse>> AddRoleActionAuthorizationAsync(SaveRoleActionAuthorizationRequest postRoleActionAuthorizationRequest);
    Task<CommonResponse<SaveRoleActionAuthorizationResponse>> EditRoleActionAuthorizationAsync(SaveRoleActionAuthorizationRequest putRoleActionAuthorizationRequest);
    Task<CommonResponse<long>> DeleteRoleActionAuthorizationAsync(long id);
}