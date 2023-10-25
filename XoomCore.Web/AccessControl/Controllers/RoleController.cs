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
public class RoleController : Controller
{
    private readonly ILogger<RoleController> _logger;

    private readonly IRoleService _RoleService;
    private readonly ISessionManager _sessionManager;
    public RoleController(ILogger<RoleController> logger,
        IRoleService RoleService,
        ISessionManager sessionManager
        )
    {
        _logger = logger;
        _RoleService = RoleService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "Role", Controller: "Role", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/AccessControl/Views/Role/Index.cshtml");
    }

    [Route("GetPermittedRole")]
    [MustHavePermission(SubMenuKey: "Role", Controller: "Role", Action: "Index")]
    public async Task<CommonResponse<ActionPermission>> GetPermittedSubRole()
    {
        var actions = new[] { "GetRoleList", "PostRole", "PutRole", "DeleteRole" };
        var permissions = await _sessionManager.CheckPermissionsAsync("Role", actions);

        ActionPermission actionPermission = new ActionPermission
        {
            View = permissions.GetValueOrDefault("GetRoleList", false),
            Create = permissions.GetValueOrDefault("PostRole", false),
            Edit = permissions.GetValueOrDefault("PutRole", false),
            Delete = permissions.GetValueOrDefault("DeleteRole", false)
        };
        return CommonResponse<ActionPermission>.CreateHappyResponse(actionPermission);
    }


    /*****************************
        
        Role Related Action Start

    *****************************/

    [Route("GetRoleList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "Role", Controller: "Role", Action: "GetRoleList")]
    public async Task<CommonDataTableResponse<IEnumerable<RoleVM>>> GetRoleList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _RoleService.GetRoleListAsync(getDataTableRequest);
    }
    [Route("GetRoleListForSelect")]
    [HttpGet]
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetRoleListForSelect()
    {
        return await _RoleService.GetRoleListForSelectAsync();
    }
    [HttpGet]
    [Route("GetRole/{id}")]
    [MustHavePermission(SubMenuKey: "Role", Controller: "Role", Action: "GetRole")]
    public async Task<CommonResponse<RoleVM>> GetRole(long id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<RoleVM>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _RoleService.GetRoleAsync(id);
    }

    [HttpPost]
    [Route("PostRole")]
    [MustHavePermission(SubMenuKey: "Role", Controller: "Role", Action: "PostRole")]
    public async Task<CommonResponse<long>> PostRole([FromBody] SaveRoleRequest postRoleRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _RoleService.AddRoleAsync(postRoleRequest);
    }

    [HttpPut]
    [Route("PutRole")]
    [MustHavePermission(SubMenuKey: "Role", Controller: "Role", Action: "PutRole")]
    public async Task<CommonResponse<long>> PutRole([FromBody] SaveRoleRequest putRoleRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _RoleService.EditRoleAsync(putRoleRequest);
    }

    [HttpDelete]
    [Route("DeleteRole/{id}")]
    [MustHavePermission(SubMenuKey: "Role", Controller: "Role", Action: "DeleteRole")]
    public async Task<CommonResponse<long>> DeleteRole(long? id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _RoleService.DeleteRoleAsync(Convert.ToInt64(id));
    }

    /*****************************

       Role Related Action End

    *****************************/
}
