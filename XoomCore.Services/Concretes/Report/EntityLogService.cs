using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XoomCore.Application.RequestModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.Report;
using XoomCore.Core.Entities.Report;
using XoomCore.Infrastructure.UnitOfWorks;
using XoomCore.Services.Contracts.Report;
using XoomCore.Services.SessionControl;

namespace XoomCore.Services.Concretes.Report;

public class EntityLogService : IEntityLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;
    public EntityLogService(IUnitOfWork unitOfWork, ISessionManager sessionManager, IMapper iMapper)
    {
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<EntityLogVM>>> GetEntityLogListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getEntityLogListQuery = _unitOfWork.EntityLogRepository
                    .GetAll()
                    .Where(x => x.EntityName.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getEntityLogListQuery.CountAsync();

        List<EntityLog> responseEntityLogList = await getEntityLogListQuery
                                    .Include(x => x.CreatedByUser)
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseEntityLogList.Any())
        {
            return CommonDataTableResponse<IEnumerable<EntityLogVM>>.CreateWarningResponse();
        }

        IEnumerable<EntityLogVM> mappedEntityLogList = _iMapper.Map<List<EntityLogVM>>(responseEntityLogList);

        return CommonDataTableResponse<IEnumerable<EntityLogVM>>.CreateHappyResponse(totalRowCount, mappedEntityLogList.OrderByDescending(x => x.Id));
    }
}
