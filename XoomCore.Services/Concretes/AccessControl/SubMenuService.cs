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

public class SubMenuService : ISubMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;
    public SubMenuService(IUnitOfWork unitOfWork, ISessionManager sessionManager, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<SubMenuVM>>> GetSubMenuListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getSubMenuListQuery = _unitOfWork.SubMenuRepository
                    .GetAll()
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getSubMenuListQuery.CountAsync();

        List<SubMenu> responseSubMenuList = await getSubMenuListQuery
                                    .Include("Menu")
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseSubMenuList.Any())
        {
            return CommonDataTableResponse<IEnumerable<SubMenuVM>>.CreateWarningResponse();
        }

        IEnumerable<SubMenuVM> mappedSubMenuList = _iMapper.Map<List<SubMenuVM>>(responseSubMenuList);

        return CommonDataTableResponse<IEnumerable<SubMenuVM>>.CreateHappyResponse(totalRowCount, mappedSubMenuList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetSubMenuListForSelectAsync()
    {
        List<SubMenu> responseSubMenuList = await _unitOfWork.SubMenuRepository.GetAll().ToListAsync();
        if (!responseSubMenuList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarningResponse();
        }

        IEnumerable<SelectOptionResponse> mappedSubMenuList = _iMapper.Map<List<SelectOptionResponse>>(responseSubMenuList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateHappyResponse(mappedSubMenuList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<SubMenuVM>> GetSubMenuAsync(long id)
    {
        SubMenu responseSubMenu = await _unitOfWork.SubMenuRepository.GetAsync(id);
        if (responseSubMenu == null)
        {
            return CommonResponse<SubMenuVM>.CreateWarningResponse();
        }

        SubMenuVM mappedSubMenu = _iMapper.Map<SubMenuVM>(responseSubMenu);
        return CommonResponse<SubMenuVM>.CreateHappyResponse(mappedSubMenu);
    }

    public async Task<CommonResponse<long>> AddSubMenuAsync(SaveSubMenuRequest createSubMenuRequest)
    {
        SubMenu mappedSubMenu = _iMapper.Map<SubMenu>(createSubMenuRequest);
        _sessionManager.SetInsertedIdentity(mappedSubMenu);

        await _unitOfWork.SubMenuRepository.InsertAsync(mappedSubMenu);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(mappedSubMenu.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> EditSubMenuAsync(SaveSubMenuRequest updateSubMenuRequest)
    {
        SubMenu existSubMenu = await _unitOfWork.SubMenuRepository.GetAsync(updateSubMenuRequest.Id);
        if (existSubMenu == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        SubMenu oldValue = existSubMenu;
        SubMenu newValue = _iMapper.Map(updateSubMenuRequest, existSubMenu);
        //updateSubMenuRequest.MapTo(existSubMenu);

        _sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.SubMenuRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existSubMenu.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteSubMenuAsync(long id)
    {
        SubMenu existSubMenu = await _unitOfWork.SubMenuRepository.GetAsync(id);
        if (existSubMenu == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        _sessionManager.SetDeletedIdentity(existSubMenu);
        await _unitOfWork.SubMenuRepository.DeleteAsync(existSubMenu);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existSubMenu.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }
}