using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;

namespace XoomCore.Services.Contracts.AccessControl;

public interface IUserService
{
    Task<CommonDataTableResponse<IEnumerable<UserVM>>> GetUserListAsync(GetDataTableRequest getDataTableRequest);
    Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetUserListForSelectAsync();
    Task<CommonResponse<UserVM>> GetUserAsync(long id);
    Task<CommonResponse<long>> AddUserAsync(SaveUserRequest postUserRequest);
    Task<CommonResponse<long>> EditUserAsync(SaveUserRequest putUserRequest);
    Task<CommonResponse<long>> ChangeUserPasswordAsync(ChangeUserPasswordRequest changeUserPasswordRequest);
    Task<CommonResponse<long>> DeleteUserAsync(long id);

    Task<CommonResponse<SignInResponse>> CreateSessionAsync(SignInRequest signInRequest);
}