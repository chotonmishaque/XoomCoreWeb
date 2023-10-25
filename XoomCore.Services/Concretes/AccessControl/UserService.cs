using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels;
using XoomCore.Application.ViewModels.AccessControl;
using XoomCore.Core.Entities.AccessControl;
using XoomCore.Core.Enum;
using XoomCore.Infrastructure.Caching;
using XoomCore.Infrastructure.Helpers;
using XoomCore.Infrastructure.UnitOfWorks;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.SessionControl;

namespace XoomCore.Services.Concretes.AccessControl;

public class UserService : IUserService
{
    private readonly ICacheManager _cacheManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionManager _sessionManager;
    private readonly IMapper _iMapper;
    public UserService(
        ICacheManager cacheManager,
        IUnitOfWork unitOfWork,
        ISessionManager sessionManager,
        IMapper iMapper)
    {
        _cacheManager = cacheManager;
        _unitOfWork = unitOfWork;
        _sessionManager = sessionManager;
        _iMapper = iMapper;
    }

    public async Task<CommonDataTableResponse<IEnumerable<UserVM>>> GetUserListAsync(GetDataTableRequest getDataTableRequest)
    {
        var getUserListQuery = _unitOfWork.UserRepository
                    .GetAll()
                    .Where(x => x.Username.Contains(getDataTableRequest.SearchText) || string.IsNullOrEmpty(getDataTableRequest.SearchText));

        long totalRowCount = await getUserListQuery.CountAsync();

        List<User> responseUserList = await getUserListQuery
                                    .OrderByDescending(x => x.Id)
                                    .Skip(getDataTableRequest.StartFrom - 1)
                                    .Take(getDataTableRequest.NoOfRecordsToFetch)
                                    .ToListAsync();
        if (!responseUserList.Any())
        {
            return CommonDataTableResponse<IEnumerable<UserVM>>.CreateWarningResponse();
        }

        IEnumerable<UserVM> mappedUserList = _iMapper.Map<List<UserVM>>(responseUserList);

        return CommonDataTableResponse<IEnumerable<UserVM>>.CreateHappyResponse(totalRowCount, mappedUserList.OrderByDescending(x => x.Id));
    }

    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetUserListForSelectAsync()
    {
        List<User> responseUserList = await _unitOfWork.UserRepository
                                    .GetAll()
                                    .Where(x => x.Status == UserStatus.IsActive)
                                    .ToListAsync();
        if (!responseUserList.Any())
        {
            return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateWarningResponse();
        }

        IEnumerable<SelectOptionResponse> mappedUserList = _iMapper.Map<List<SelectOptionResponse>>(responseUserList);
        return CommonResponse<IEnumerable<SelectOptionResponse>>.CreateHappyResponse(mappedUserList.OrderBy(x => x.Name));
    }

    public async Task<CommonResponse<UserVM>> GetUserAsync(long id)
    {
        User responseUser = await _unitOfWork.UserRepository.GetAsync(id);
        if (responseUser == null)
        {
            return CommonResponse<UserVM>.CreateWarningResponse();
        }

        UserVM mappedUser = _iMapper.Map<UserVM>(responseUser);
        return CommonResponse<UserVM>.CreateHappyResponse(mappedUser);
    }

    public async Task<CommonResponse<long>> AddUserAsync(SaveUserRequest createUserRequest)
    {
        User mappedUser = _iMapper.Map<User>(createUserRequest);
        mappedUser.Password = SecurePasswordHasher.Hash("XoomCore@123");
        _sessionManager.SetInsertedIdentity(mappedUser);

        await _unitOfWork.UserRepository.InsertAsync(mappedUser);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(mappedUser.Id, "Saved successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> EditUserAsync(SaveUserRequest updateUserRequest)
    {
        User existUser = await _unitOfWork.UserRepository.GetAsync(updateUserRequest.Id);
        if (existUser == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        User oldValue = existUser;
        User newValue = _iMapper.Map(updateUserRequest, existUser);

        _sessionManager.SetUpdatedIdentity(newValue);

        await _unitOfWork.UserRepository.UpdateAsync(oldValue, newValue);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existUser.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> ChangeUserPasswordAsync(ChangeUserPasswordRequest changeUserPasswordRequest)
    {
        User existUser = await _unitOfWork.UserRepository.GetAsync(changeUserPasswordRequest.Id);
        if (existUser == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        existUser.Password = SecurePasswordHasher.Hash(changeUserPasswordRequest.NewPassword);
        _sessionManager.SetUpdatedIdentity(existUser);

        await _unitOfWork.UserRepository.UpdateAsync(existUser);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existUser.Id, "Updated successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<long>> DeleteUserAsync(long id)
    {
        User existUser = await _unitOfWork.UserRepository.GetAsync(id);
        if (existUser == null)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        _sessionManager.SetDeletedIdentity(existUser);
        await _unitOfWork.UserRepository.DeleteAsync(existUser);
        int dbCode = await _unitOfWork.SaveChangesAsync();

        if (dbCode > 0)
        {
            return CommonResponse<long>.CreateHappyResponse(existUser.Id, "Deleted successfully.");
        }
        return CommonResponse<long>.CreateUnhappyResponse(dbCode);
    }

    public async Task<CommonResponse<SignInResponse>> CreateSessionAsync(SignInRequest signInRequest)
    {
        try
        {
            User responseUser = await _unitOfWork.UserRepository.Get(x => x.Email == signInRequest.Email).SingleOrDefaultAsync();
            if (responseUser == null)
            {
                return CommonResponse<SignInResponse>.CreateUnhappyResponse(errorCode: (int)HttpStatusCode.Unauthorized, errorMessage: "Invalid Credentials");
            }
            if (responseUser.Status != UserStatus.IsActive)
            {
                return CommonResponse<SignInResponse>.CreateUnhappyResponse(errorCode: (int)HttpStatusCode.Unauthorized, errorMessage: "Your account is not Active");
            }
            if (!SecurePasswordHasher.Verify(signInRequest.Password, responseUser.Password))
            {
                return CommonResponse<SignInResponse>.CreateUnhappyResponse(errorCode: (int)HttpStatusCode.Unauthorized, errorMessage: "Invalid Credentials");
            }

            var responseUserForSession = await GetUserForSessionAsync(responseUser.Id);
            if (responseUserForSession == null)
            {
                return CommonResponse<SignInResponse>.CreateUnhappyResponse(errorCode: (int)HttpStatusCode.Unauthorized, errorMessage: "Something went wrong!");
            }
            var sessionData = new SessionData
            {
                UserId = responseUserForSession.UserId,
                SubMenuAuthorizedList = responseUserForSession.SubMenuAuthorizedList,
                ActionAuthorizedList = responseUserForSession.ActionAuthorizedList,
            };

            _sessionManager.Current = sessionData;
            var signInResponse = new SignInResponse
            {
                Url = "/index"
            };
            return CommonResponse<SignInResponse>.CreateHappyResponse(signInResponse);
        }
        catch (Exception)
        {
            return CommonResponse<SignInResponse>.CreateUnhappyResponse(errorCode: (int)HttpStatusCode.Unauthorized, errorMessage: "Something went wrong!");
        }
    }

    private async Task<SessionCreationResponseVM> GetUserForSessionAsync(long userId)
    {
        try
        {
            var userEntity = await _unitOfWork.UserRepository.Get(x => x.Id == userId)
                            .Include(u => u.UserRoles)
                                .ThenInclude(ur => ur.Role.RoleActionAuthorizations)
                                    .ThenInclude(ra => ra.ActionAuthorization)
                                        .ThenInclude(aa => aa.SubMenu.Menu)
                            .SingleOrDefaultAsync();

            if (userEntity == null)
            {
                // Handle the case where the user doesn't exist
            }

            var permittedSubMenus = userEntity.UserRoles
                .SelectMany(ur => ur.Role.RoleActionAuthorizations)
                .Where(ur => ur.ActionAuthorization.IsPageLinked == 1)
                .Where(rp => rp.Status == EntityStatus.IsActive &&
                             rp.ActionAuthorization.SubMenu.Status == EntityStatus.IsActive &&
                             rp.ActionAuthorization.SubMenu.Menu.Status == EntityStatus.IsActive)
                .GroupBy(rp => rp.ActionAuthorization.SubMenu.Id) // Group by SubMenuId
                .Select(group => group.First()) // Select the first element in each group
                .Select(rp => new SubMenuAuthorizedVM
                {
                    Menu = new MenuVM
                    {
                        Id = rp.ActionAuthorization.SubMenu.MenuId,
                        Name = rp.ActionAuthorization.SubMenu.Menu.Name,
                        DisplaySequence = rp.ActionAuthorization.SubMenu.Menu.DisplaySequence,
                        Icon = rp.ActionAuthorization.SubMenu.Menu.Icon,
                    },
                    Id = rp.ActionAuthorization.SubMenu.Id,
                    Key = rp.ActionAuthorization.SubMenu.Key,
                    Name = rp.ActionAuthorization.SubMenu.Name,
                    Url = rp.ActionAuthorization.SubMenu.Url,
                    DisplaySequence = rp.ActionAuthorization.SubMenu.DisplaySequence
                })
                .OrderBy(permissions => permissions.Menu.DisplaySequence)
                .ThenBy(permissions => permissions.DisplaySequence)
                .ToList();

            var permittedActions = userEntity.UserRoles
                .SelectMany(ur => ur.Role.RoleActionAuthorizations)
                .Where(rp => rp.Status == EntityStatus.IsActive &&
                             rp.ActionAuthorization.Status == EntityStatus.IsActive)
                .GroupBy(rp => rp.ActionAuthorization.Id) // Group by ActionAuthorization.Id
                .Select(group => group.First()) // Select the first element in each group
                .Select(rp => new ActionAuthorizedVM
                {
                    Id = rp.ActionAuthorization.Id,
                    Name = rp.ActionAuthorization.Name,
                    ControllerName = rp.ActionAuthorization.Controller,
                    ActionMethod = rp.ActionAuthorization.ActionMethod
                })
                .OrderBy(permissions => permissions.Id)
                .ToList();

            var sessionCreationResponse = new SessionCreationResponseVM
            {
                UserId = userEntity.Id,
                Username = userEntity.Username,
                //Password = userEntity.Password,
                //PermittedMenuItems = permittedMenuItems,
                SubMenuAuthorizedList = permittedSubMenus,
                ActionAuthorizedList = permittedActions,
            };

            return sessionCreationResponse;
        }
        catch (Exception)
        {
            return null;
        }
    }

}