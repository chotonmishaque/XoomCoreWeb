using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Services.Contracts.AccessControl;

public interface IMenuService
{

    Task<CommonDataTableResponse<IEnumerable<MenuVM>>> GetMenuListAsync(GetDataTableRequest getDataTableRequest);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetMenuListForSelectAsync();
    Task<CommonResponse<MenuVM>> GetMenuAsync(long id);
    Task<CommonResponse<long>> AddMenuAsync(SaveMenuRequest postMenuRequest);
    Task<CommonResponse<long>> EditMenuAsync(SaveMenuRequest putMenuRequest);
    Task<CommonResponse<long>> DeleteMenuAsync(long id);

}
