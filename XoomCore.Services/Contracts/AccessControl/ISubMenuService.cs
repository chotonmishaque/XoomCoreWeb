using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Services.Contracts.AccessControl;

public interface ISubMenuService
{
    Task<CommonDataTableResponse<IEnumerable<SubMenuVM>>> GetSubMenuListAsync(GetDataTableRequest getDataTableRequest);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetSubMenuListForSelectAsync();
    Task<CommonResponse<SubMenuVM>> GetSubMenuAsync(long id);
    Task<CommonResponse<long>> AddSubMenuAsync(SaveSubMenuRequest postSubMenuRequest);
    Task<CommonResponse<long>> EditSubMenuAsync(SaveSubMenuRequest putSubMenuRequest);
    Task<CommonResponse<long>> DeleteSubMenuAsync(long id);
}