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

public class UserRoleService : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;
    public UserRoleService(IUnitOfWork unitOfWork, ISessionManager sessionManager, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<UserRoleVM>>> GetUserRoleListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getUserRoleListQuery = _unitOfWork.UserRoleRepository
                    .GetAll()
                    .Where(x => x.Role.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getUserRoleListQuery.CountAsync();

        List<UserRole> responseUserRoleList = await getUserRoleListQuery
                                    .Include(x => x.Role)
                                    .Include(x => x.User)
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseUserRoleList.Any())
        {
            return CommonDataTableResponse<IEnumerable<UserRoleVM>>.CreateWarningResponse();
        }

        IEnumerable<UserRoleVM> mappedUserRoleList = _iMapper.Map<List<UserRoleVM>>(responseUserRoleList);

        return CommonDataTableResponse<IEnumerable<UserRoleVM>>.CreateHappyResponse(totalRowCount, mappedUserRoleList.OrderByDescending(x => x.Id));
    }

    public async Task<CommonResponse<UserRoleVM>> GetUserRoleAsync(long id)
    {
        UserRole responseUserRole = await _unitOfWork.UserRoleRepository.GetAsync(id);
        if (responseUserRole == null)
        {
            return CommonResponse<UserRoleVM>.CreateWarningResponse();
        }

        UserRoleVM mappedUserRole = _iMapper.Map<UserRoleVM>(responseUserRole);
        return CommonResponse<UserRoleVM>.CreateHappyResponse(mappedUserRole);
    }

    public async Task<CommonResponse<long>> AddUserRoleAsync(SaveUserRoleRequest createUserRoleRequest)
    {
        UserRole mappedUserRole = _iMapper.Map<UserRole>(createUserRoleRequest);
        _sessionManager.SetInsertedIdentity(mappedUserRole);

        await _unitOfWork.UserRoleRepository.InsertAsync(mappedUserRole);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(mappedUserRole.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> EditUserRoleAsync(SaveUserRoleRequest updateUserRoleRequest)
    {
        UserRole existUserRole = await _unitOfWork.UserRoleRepository.GetAsync(updateUserRoleRequest.Id);
        if (existUserRole == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        UserRole oldValue = existUserRole;
        UserRole newValue = _iMapper.Map(updateUserRoleRequest, existUserRole);
        //updateUserRoleRequest.MapTo(existUserRole);

        _sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.UserRoleRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existUserRole.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteUserRoleAsync(long id)
    {
        UserRole existUserRole = await _unitOfWork.UserRoleRepository.GetAsync(id);
        if (existUserRole == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }

        await _unitOfWork.UserRoleRepository.DeleteAsync(existUserRole);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existUserRole.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }
}