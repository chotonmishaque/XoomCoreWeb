using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;
using XoomCore.Core.Entities.AccessControl;
using XoomCore.Core.Enum;
using XoomCore.Infrastructure.UnitOfWorks;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.SessionControl;

namespace XoomCore.Services.Concretes.AccessControl;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;

    public RoleService(IUnitOfWork unitOfWork, ISessionManager sessionManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = mapper;
    }
    public async Task<CommonDataTableResponse<IEnumerable<RoleVM>>> GetRoleListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getRoleListQuery = _unitOfWork.RoleRepository
                    .GetAll()
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getRoleListQuery.CountAsync();

        List<Role> responseRoleList = await getRoleListQuery
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseRoleList.Any())
        {
            return CommonDataTableResponse<IEnumerable<RoleVM>>.CreateWarningResponse();
        }

        IEnumerable<RoleVM> mappedRoleList = _iMapper.Map<List<RoleVM>>(responseRoleList);

        return CommonDataTableResponse<IEnumerable<RoleVM>>.CreateHappyResponse(totalRowCount, mappedRoleList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetRoleListForSelectAsync()
    {
        List<Role> respRoleList = await _unitOfWork.RoleRepository
                                .GetAll()
                                .Where(x => x.Status == EntityStatus.IsActive)
                                .ToListAsync();
        if (!respRoleList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarningResponse();
        }

        IEnumerable<SelectOptionResponse> mappedRoleCategories = _iMapper.Map<List<SelectOptionResponse>>(respRoleList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateHappyResponse(mappedRoleCategories.OrderByDescending(x => x.Id));
    }

    public async Task<CommonResponse<RoleVM>> GetRoleAsync(long id)
    {
        var respRole = await _unitOfWork.RoleRepository.GetAsync(id);
        if (respRole == null)
        {
            return CommonResponse<RoleVM>.CreateWarningResponse();
        }

        RoleVM mappedRole = _iMapper.Map<RoleVM>(respRole);
        return CommonResponse<RoleVM>.CreateHappyResponse(mappedRole);
    }

    public async Task<CommonResponse<long>> AddRoleAsync(SaveRoleRequest createRoleRequest)
    {
        Role mappedRole = _iMapper.Map<Role>(createRoleRequest);
        _sessionManager.SetInsertedIdentity(mappedRole);

        await _unitOfWork.RoleRepository.InsertAsync(mappedRole);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(mappedRole.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> EditRoleAsync(SaveRoleRequest updateRoleRequest)
    {
        Role existRole = await _unitOfWork.RoleRepository.GetAsync(updateRoleRequest.Id);
        if (existRole == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        Role oldValue = existRole;
        Role newValue = _iMapper.Map(updateRoleRequest, existRole);

        _sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.RoleRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existRole.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteRoleAsync(long id)
    {
        Role existRole = await _unitOfWork.RoleRepository.GetAsync(id);
        if (existRole == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }

        await _unitOfWork.RoleRepository.DeleteAsync(existRole);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existRole.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }
}
