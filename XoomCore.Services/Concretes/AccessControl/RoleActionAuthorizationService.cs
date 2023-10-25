using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;
using XoomCore.Core.Entities.AccessControl;
using XoomCore.Infrastructure.UnitOfWorks;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.SessionControl;

namespace XoomCore.Services.Concretes.AccessControl;

public class RoleActionAuthorizationService : IRoleActionAuthorizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;
    public RoleActionAuthorizationService(IUnitOfWork unitOfWork, ISessionManager sessionManager, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<RoleActionAuthorizationVM>>> GetRoleActionAuthorizationListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getRoleActionAuthorizationListQuery = _unitOfWork.RoleActionAuthorizationRepository
                    .GetAll()
                    .Where(x => x.Role.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getRoleActionAuthorizationListQuery.CountAsync();

        List<RoleActionAuthorization> responseRoleActionAuthorizationList = await getRoleActionAuthorizationListQuery
                                    .Include(x => x.Role)
                                    .Include(x => x.ActionAuthorization)
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseRoleActionAuthorizationList.Any())
        {
            return CommonDataTableResponse<IEnumerable<RoleActionAuthorizationVM>>.CreateWarningResponse();
        }

        IEnumerable<RoleActionAuthorizationVM> mappedRoleActionAuthorizationList = _iMapper.Map<List<RoleActionAuthorizationVM>>(responseRoleActionAuthorizationList);

        return CommonDataTableResponse<IEnumerable<RoleActionAuthorizationVM>>.CreateHappyResponse(totalRowCount, mappedRoleActionAuthorizationList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetRoleActionAuthorizationListForSelectAsync()
    {
        List<RoleActionAuthorization> responseRoleActionAuthorizationList = await _unitOfWork.RoleActionAuthorizationRepository.GetAll().ToListAsync();
        if (!responseRoleActionAuthorizationList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarningResponse();
        }

        IEnumerable<SelectOptionResponse> mappedRoleActionAuthorizationList = _iMapper.Map<List<SelectOptionResponse>>(responseRoleActionAuthorizationList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateHappyResponse(mappedRoleActionAuthorizationList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<RoleActionAuthorizationVM>> GetRoleActionAuthorizationAsync(long id)
    {
        RoleActionAuthorization responseRoleActionAuthorization = await _unitOfWork.RoleActionAuthorizationRepository.GetAsync(id);
        if (responseRoleActionAuthorization == null)
        {
            return CommonResponse<RoleActionAuthorizationVM>.CreateWarningResponse();
        }

        RoleActionAuthorizationVM mappedRoleActionAuthorization = _iMapper.Map<RoleActionAuthorizationVM>(responseRoleActionAuthorization);
        return CommonResponse<RoleActionAuthorizationVM>.CreateHappyResponse(mappedRoleActionAuthorization);
    }

    public async Task<CommonResponse<SaveRoleActionAuthorizationResponse>> AddRoleActionAuthorizationAsync(SaveRoleActionAuthorizationRequest createRoleActionAuthorizationRequest)
    {
        RoleActionAuthorization mappedRoleActionAuthorization = _iMapper.Map<RoleActionAuthorization>(createRoleActionAuthorizationRequest);
        _sessionManager.SetInsertedIdentity(mappedRoleActionAuthorization);

        await _unitOfWork.RoleActionAuthorizationRepository.InsertAsync(mappedRoleActionAuthorization);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            var response = new SaveRoleActionAuthorizationResponse
            {
                Id = mappedRoleActionAuthorization.Id
            };
            return CommonResponse<SaveRoleActionAuthorizationResponse>.CreateHappyResponse(response, "Saved successfully.");
        }
        return CommonResponse<SaveRoleActionAuthorizationResponse>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<SaveRoleActionAuthorizationResponse>> EditRoleActionAuthorizationAsync(SaveRoleActionAuthorizationRequest updateRoleActionAuthorizationRequest)
    {
        RoleActionAuthorization existRoleActionAuthorization = await _unitOfWork.RoleActionAuthorizationRepository.GetAsync(updateRoleActionAuthorizationRequest.Id);
        if (existRoleActionAuthorization == null)
        {
            return CommonResponse<SaveRoleActionAuthorizationResponse>.CreateWarningResponse(message: "Invalid request detected!");
        }
        RoleActionAuthorization oldValue = existRoleActionAuthorization;
        RoleActionAuthorization newValue = _iMapper.Map(updateRoleActionAuthorizationRequest, existRoleActionAuthorization);
        //updateRoleActionAuthorizationRequest.MapTo(existRoleActionAuthorization);

        _sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.RoleActionAuthorizationRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            var response = new SaveRoleActionAuthorizationResponse
            {
                Id = existRoleActionAuthorization.Id
            };
            return CommonResponse<SaveRoleActionAuthorizationResponse>.CreateHappyResponse(response, "Updated successfully.");
        }
        return CommonResponse<SaveRoleActionAuthorizationResponse>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteRoleActionAuthorizationAsync(long id)
    {
        RoleActionAuthorization existRoleActionAuthorization = await _unitOfWork.RoleActionAuthorizationRepository.GetAsync(id);
        if (existRoleActionAuthorization == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }

        await _unitOfWork.RoleActionAuthorizationRepository.DeleteAsync(existRoleActionAuthorization);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existRoleActionAuthorization.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }
}
