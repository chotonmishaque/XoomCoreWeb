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

public class ActionAuthorizationService : IActionAuthorizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;
    public ActionAuthorizationService(IUnitOfWork unitOfWork, ISessionManager sessionManager, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<ActionAuthorizationVM>>> GetActionAuthorizationListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getActionAuthorizationListQuery = _unitOfWork.ActionAuthorizationRepository
                    .GetAll()
                    .Where(x => x.Name.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getActionAuthorizationListQuery.CountAsync();

        List<ActionAuthorization> responseActionAuthorizationList = await getActionAuthorizationListQuery
                                    .Include(x => x.SubMenu)
                                        .ThenInclude(x => x.Menu)
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseActionAuthorizationList.Any())
        {
            return CommonDataTableResponse<IEnumerable<ActionAuthorizationVM>>.CreateWarningResponse();
        }

        IEnumerable<ActionAuthorizationVM> mappedActionAuthorizationList = _iMapper.Map<List<ActionAuthorizationVM>>(responseActionAuthorizationList);

        return CommonDataTableResponse<IEnumerable<ActionAuthorizationVM>>.CreateHappyResponse(totalRowCount, mappedActionAuthorizationList.OrderByDescending(x => x.Id));
    }
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetActionAuthorizationListForSelectAsync()
    {
        List<ActionAuthorization> responseActionAuthorizationList = await _unitOfWork.ActionAuthorizationRepository.GetAll().ToListAsync();
        if (!responseActionAuthorizationList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarningResponse();
        }

        IEnumerable<SelectOptionResponse> mappedActionAuthorizationList = _iMapper.Map<List<SelectOptionResponse>>(responseActionAuthorizationList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateHappyResponse(mappedActionAuthorizationList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<ActionAuthorizationVM>> GetActionAuthorizationAsync(long id)
    {
        ActionAuthorization responseActionAuthorization = await _unitOfWork.ActionAuthorizationRepository.GetAsync(id);
        if (responseActionAuthorization == null)
        {
            return CommonResponse<ActionAuthorizationVM>.CreateWarningResponse();
        }

        ActionAuthorizationVM mappedActionAuthorization = _iMapper.Map<ActionAuthorizationVM>(responseActionAuthorization);
        return CommonResponse<ActionAuthorizationVM>.CreateHappyResponse(mappedActionAuthorization);
    }

    public async Task<CommonResponse<SaveActionAuthorizationResponse>> AddActionAuthorizationAsync(SaveActionAuthorizationRequest createActionAuthorizationRequest)
    {
        ActionAuthorization mappedActionAuthorization = _iMapper.Map<ActionAuthorization>(createActionAuthorizationRequest);
        _sessionManager.SetInsertedIdentity(mappedActionAuthorization);

        await _unitOfWork.ActionAuthorizationRepository.InsertAsync(mappedActionAuthorization);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            var response = new SaveActionAuthorizationResponse
            {
                Id = mappedActionAuthorization.Id
            };
            return CommonResponse<SaveActionAuthorizationResponse>.CreateHappyResponse(response, "Saved successfully.");
        }
        return CommonResponse<SaveActionAuthorizationResponse>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<SaveActionAuthorizationResponse>> EditActionAuthorizationAsync(SaveActionAuthorizationRequest updateActionAuthorizationRequest)
    {
        ActionAuthorization existActionAuthorization = await _unitOfWork.ActionAuthorizationRepository.GetAsync(updateActionAuthorizationRequest.Id);
        if (existActionAuthorization == null)
        {
            return CommonResponse<SaveActionAuthorizationResponse>.CreateWarningResponse(message: "Invalid request detected!");
        }
        ActionAuthorization oldValue = existActionAuthorization;
        ActionAuthorization newValue = _iMapper.Map(updateActionAuthorizationRequest, existActionAuthorization);
        //updateActionAuthorizationRequest.MapTo(existActionAuthorization);

        _sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.ActionAuthorizationRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            var response = new SaveActionAuthorizationResponse
            {
                Id = existActionAuthorization.Id
            };
            return CommonResponse<SaveActionAuthorizationResponse>.CreateHappyResponse(response, "Updated successfully.");
        }
        return CommonResponse<SaveActionAuthorizationResponse>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteActionAuthorizationAsync(long id)
    {
        ActionAuthorization existActionAuthorization = await _unitOfWork.ActionAuthorizationRepository.GetAsync(id);
        if (existActionAuthorization == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }

        await _unitOfWork.ActionAuthorizationRepository.DeleteAsync(existActionAuthorization);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existActionAuthorization.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }
}
