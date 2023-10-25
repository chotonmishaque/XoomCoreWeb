using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Services.Contracts.AccessControl;


public interface IActionAuthorizationService
{
    Task<CommonDataTableResponse<IEnumerable<ActionAuthorizationVM>>> GetActionAuthorizationListAsync(GetDataTableRequest getDataTableRequest);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetActionAuthorizationListForSelectAsync();
    Task<CommonResponse<ActionAuthorizationVM>> GetActionAuthorizationAsync(long id);
    Task<CommonResponse<SaveActionAuthorizationResponse>> AddActionAuthorizationAsync(SaveActionAuthorizationRequest postActionAuthorizationRequest);
    Task<CommonResponse<SaveActionAuthorizationResponse>> EditActionAuthorizationAsync(SaveActionAuthorizationRequest putActionAuthorizationRequest);
    Task<CommonResponse<long>> DeleteActionAuthorizationAsync(long id);
}