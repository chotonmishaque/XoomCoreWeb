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
public class UserRoleController : Controller
{
    private readonly ILogger<UserRoleController> _logger;

    private readonly IUserRoleService _UserRoleService;
    private readonly ISessionManager _sessionManager;
    public UserRoleController(ILogger<UserRoleController> logger,
        IUserRoleService UserRoleService,
        ISessionManager sessionManager
        )
    {
        _logger = logger;
        _UserRoleService = UserRoleService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "UserRole", Controller: "UserRole", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/AccessControl/Views/UserRole/Index.cshtml");
    }

    [Route("GetActionPermission")]
    [MustHavePermission(SubMenuKey: "UserRole", Controller: "UserRole", Action: "Index")]
    public async Task<CommonResponse<dynamic>> GetActionPermission()
    {
        var actions = new[] { "GetUserRoleList", "PostUserRole", "PutUserRole", "DeleteUserRole" };
        var permissions = await _sessionManager.CheckPermissionsAsync("UserRole", actions);

        var actionPermission = new
        {
            View = permissions.GetValueOrDefault("GetUserRoleList", false),
            Create = permissions.GetValueOrDefault("PostUserRole", false),
            Edit = permissions.GetValueOrDefault("PutUserRole", false),
            Delete = permissions.GetValueOrDefault("DeleteUserRole", false)
        };
        return CommonResponse<dynamic>.CreateHappyResponse(actionPermission);
    }


    /*****************************
        
        UserRole Related Action Start

    *****************************/

    [Route("GetUserRoleList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "UserRole", Controller: "UserRole", Action: "GetUserRoleList")]
    public async Task<CommonDataTableResponse<IEnumerable<UserRoleVM>>> GetUserRoleList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _UserRoleService.GetUserRoleListAsync(getDataTableRequest);
    }
    [HttpGet]
    [Route("GetUserRole/{id}")]
    [MustHavePermission(SubMenuKey: "UserRole", Controller: "UserRole", Action: "GetUserRole")]
    public async Task<CommonResponse<UserRoleVM>> GetUserRole(long id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<UserRoleVM>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _UserRoleService.GetUserRoleAsync(id);
    }

    [HttpPost]
    [Route("PostUserRole")]
    [MustHavePermission(SubMenuKey: "UserRole", Controller: "UserRole", Action: "PostUserRole")]
    public async Task<CommonResponse<long>> PostUserRole([FromBody] SaveUserRoleRequest postUserRoleRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _UserRoleService.AddUserRoleAsync(postUserRoleRequest);
    }

    [HttpPut]
    [Route("PutUserRole")]
    [MustHavePermission(SubMenuKey: "UserRole", Controller: "UserRole", Action: "PutUserRole")]
    public async Task<CommonResponse<long>> PutUserRole([FromBody] SaveUserRoleRequest putUserRoleRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _UserRoleService.EditUserRoleAsync(putUserRoleRequest);
    }

    [HttpDelete]
    [Route("DeleteUserRole/{id}")]
    [MustHavePermission(SubMenuKey: "UserRole", Controller: "UserRole", Action: "DeleteUserRole")]
    public async Task<CommonResponse<long>> DeleteUserRole(long? id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _UserRoleService.DeleteUserRoleAsync(Convert.ToInt64(id));
    }

    /*****************************

       UserRole Related Action End

    *****************************/
}
