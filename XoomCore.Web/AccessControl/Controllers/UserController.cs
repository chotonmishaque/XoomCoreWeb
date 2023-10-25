using Microsoft.AspNetCore.Mvc;
using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.SessionControl;

namespace XoomCore.Web.AccessControl.Controllers;


[MustHaveAuthorize]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;

    private readonly IUserService _UserService;
    private readonly ISessionManager _sessionManager;
    public UserController(ILogger<UserController> logger,
        IUserService UserService,
        ISessionManager sessionManager
        )
    {
        _logger = logger;
        _UserService = UserService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/AccessControl/Views/User/Index.cshtml");
    }

    [Route("GetActionPermission")]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "Index")]
    public async Task<CommonResponse<dynamic>> GetPermittedSubUser()
    {
        var actions = new[] { "GetUserList", "PostUser", "PutUser", "ChangeUserPassword", "DeleteUser" };
        var permissions = await _sessionManager.CheckPermissionsAsync("User", actions);

        var actionPermission = new
        {
            View = permissions.GetValueOrDefault("GetUserList", false),
            Create = permissions.GetValueOrDefault("PostUser", false),
            Edit = permissions.GetValueOrDefault("PutUser", false),
            ChangeUserPassword = permissions.GetValueOrDefault("ChangeUserPassword", false),
            Delete = permissions.GetValueOrDefault("DeleteUser", false)
        };
        return CommonResponse<dynamic>.CreateHappyResponse(actionPermission);
    }

    /*****************************
        
        User Related Action Start

    *****************************/

    [Route("GetUserList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "GetUserList")]
    public async Task<CommonDataTableResponse<IEnumerable<UserVM>>> GetUserList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _UserService.GetUserListAsync(getDataTableRequest);
    }
    [Route("GetUserListForSelect")]
    [HttpGet]
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetUserListForSelect()
    {
        return await _UserService.GetUserListForSelectAsync();
    }
    [HttpGet]
    [Route("GetUser/{id}")]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "GetUser")]
    public async Task<CommonResponse<UserVM>> GetUser(long id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<UserVM>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _UserService.GetUserAsync(id);
    }

    [HttpPost]
    [Route("PostUser")]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "PostUser")]
    public async Task<CommonResponse<long>> PostUser([FromBody] SaveUserRequest postUserRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _UserService.AddUserAsync(postUserRequest);
    }

    [HttpPut]
    [Route("PutUser")]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "PutUser")]
    public async Task<CommonResponse<long>> PutUser([FromBody] SaveUserRequest putUserRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _UserService.EditUserAsync(putUserRequest);
    }
    [HttpPut]
    [Route("ChangeUserPassword")]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "ChangeUserPassword")]
    public async Task<CommonResponse<long>> ChangeUserPassword([FromBody] ChangeUserPasswordRequest changeUserPasswordRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _UserService.ChangeUserPasswordAsync(changeUserPasswordRequest);
    }

    [HttpDelete]
    [Route("DeleteUser/{id}")]
    [MustHavePermission(SubMenuKey: "User", Controller: "User", Action: "DeleteUser")]
    public async Task<CommonResponse<long>> DeleteUser(long? id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _UserService.DeleteUserAsync(Convert.ToInt64(id));
    }

    /*****************************

       User Related Action End

    *****************************/
}

