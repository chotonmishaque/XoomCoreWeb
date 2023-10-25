using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;
using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.UnitOfWorks;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.SessionControl;

namespace XoomCore.Services.Concretes.AccessControl;

public class MenuService : IMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;
    public MenuService(IUnitOfWork unitOfWork, ISessionManager sessionManager, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<MenuVM>>> GetMenuListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getMenuListQuery = _unitOfWork.MenuRepository
                    .GetAll()
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getMenuListQuery.CountAsync();

        List<Menu> responseMenuList = await getMenuListQuery
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseMenuList.Any())
        {
            return CommonDataTableResponse<IEnumerable<MenuVM>>.CreateWarningResponse();
        }

        IEnumerable<MenuVM> mappedMenuList = _iMapper.Map<List<MenuVM>>(responseMenuList);

        return CommonDataTableResponse<IEnumerable<MenuVM>>.CreateHappyResponse(totalRowCount, mappedMenuList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetMenuListForSelectAsync()
    {
        List<Menu> responseMenuList = await _unitOfWork.MenuRepository.GetAll().ToListAsync();
        if (!responseMenuList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarningResponse();
        }

        IEnumerable<SelectOptionResponse> mappedMenuList = _iMapper.Map<List<SelectOptionResponse>>(responseMenuList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateHappyResponse(mappedMenuList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<MenuVM>> GetMenuAsync(long id)
    {
        Menu responseMenu = await _unitOfWork.MenuRepository.GetAsync(id);
        if (responseMenu == null)
        {
            return CommonResponse<MenuVM>.CreateWarningResponse();
        }

        MenuVM mappedMenu = _iMapper.Map<MenuVM>(responseMenu);
        return CommonResponse<MenuVM>.CreateHappyResponse(mappedMenu);
    }

    public async Task<CommonResponse<long>> AddMenuAsync(SaveMenuRequest createMenuRequest)
    {
        Menu mappedMenu = _iMapper.Map<Menu>(createMenuRequest);
        _sessionManager.SetInsertedIdentity(mappedMenu);

        await _unitOfWork.MenuRepository.InsertAsync(mappedMenu);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(mappedMenu.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> EditMenuAsync(SaveMenuRequest updateMenuRequest)
    {
        Menu existMenu = await _unitOfWork.MenuRepository.GetAsync(updateMenuRequest.Id);
        if (existMenu == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        Menu oldValue = existMenu;
        Menu newValue = _iMapper.Map(updateMenuRequest, existMenu);
        //updateMenuRequest.MapTo(existMenu);

        _sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.MenuRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existMenu.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteMenuAsync(long id)
    {
        Menu existMenu = await _unitOfWork.MenuRepository.GetAsync(id);
        if (existMenu == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }

        await _unitOfWork.MenuRepository.DeleteAsync(existMenu);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existMenu.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }
}