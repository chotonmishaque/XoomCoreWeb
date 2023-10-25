using Microsoft.AspNetCore.Mvc;
using XoomCore.Application.RequestModels;
using XoomCore.Application.RequestModels.AccessControl;
using XoomCore.Application.ResponseModels.AccessControl;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.AccessControl;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.SessionControl;

namespace XoomCore.Web.AccessControl.Controllers;


[MustHaveAuthorize]
[Route("[controller]")]
public class RoleActionAuthorizationController : Controller
{
    private readonly ILogger<RoleActionAuthorizationController> _logger;

    private readonly IRoleActionAuthorizationService _roleActionAuthorizationService;
    private readonly ISessionManager _sessionManager;
    public RoleActionAuthorizationController(ILogger<RoleActionAuthorizationController> logger,
        IRoleActionAuthorizationService RoleActionAuthorizationService,
        ISessionManager sessionManager
        )
    {
        _logger = logger;
        _roleActionAuthorizationService = RoleActionAuthorizationService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "RoleActionAuthorization", Controller: "RoleActionAuthorization", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/AccessControl/Views/RoleActionAuthorization/Index.cshtml");
    }

    [Route("GetPermittedRoleActionAuthorization")]
    [MustHavePermission(SubMenuKey: "RoleActionAuthorization", Controller: "RoleActionAuthorization", Action: "Index")]
    public async Task<CommonResponse<ActionPermission>> GetPermittedSubRoleActionAuthorization()
    {
        var actions = new[] { "GetRoleActionAuthorizationList", "PostRoleActionAuthorization", "PutRoleActionAuthorization", "DeleteRoleActionAuthorization" };
        var permissions = await _sessionManager.CheckPermissionsAsync("RoleActionAuthorization", actions);

        ActionPermission actionPermission = new ActionPermission
        {
            View = permissions.GetValueOrDefault("GetRoleActionAuthorizationList", false),
            Create = permissions.GetValueOrDefault("PostRoleActionAuthorization", false),
            Edit = permissions.GetValueOrDefault("PutRoleActionAuthorization", false),
            Delete = permissions.GetValueOrDefault("DeleteRoleActionAuthorization", false)
        };
        return CommonResponse<ActionPermission>.CreateHappyResponse(actionPermission);

    }

    /*****************************
        
        RoleActionAuthorization Related Action Start

    *****************************/

    [Route("GetRoleActionAuthorizationList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "RoleActionAuthorization", Controller: "RoleActionAuthorization", Action: "GetRoleActionAuthorizationList")]
    public async Task<CommonDataTableResponse<IEnumerable<RoleActionAuthorizationVM>>> GetRoleActionAuthorizationList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _roleActionAuthorizationService.GetRoleActionAuthorizationListAsync(getDataTableRequest);
    }

    [Route("GetRoleActionAuthorizationListForSelect")]
    [HttpGet]
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetRoleActionAuthorizationListForSelect()
    {
        return await _roleActionAuthorizationService.GetRoleActionAuthorizationListForSelectAsync();
    }
    [HttpGet]
    [Route("GetRoleActionAuthorization/{id}")]
    [MustHavePermission(SubMenuKey: "RoleActionAuthorization", Controller: "RoleActionAuthorization", Action: "GetRoleActionAuthorization")]
    public async Task<CommonResponse<RoleActionAuthorizationVM>> GetRoleActionAuthorization(long id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<RoleActionAuthorizationVM>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _roleActionAuthorizationService.GetRoleActionAuthorizationAsync(id);
    }

    [HttpPost]
    [Route("PostRoleActionAuthorization")]
    [MustHavePermission(SubMenuKey: "RoleActionAuthorization", Controller: "RoleActionAuthorization", Action: "PostRoleActionAuthorization")]
    public async Task<CommonResponse<SaveRoleActionAuthorizationResponse>> PostRoleActionAuthorization([FromBody] SaveRoleActionAuthorizationRequest postRoleActionAuthorizationRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<SaveRoleActionAuthorizationResponse>.CreateWarningResponse(message: errorMessage);
        }
        return await _roleActionAuthorizationService.AddRoleActionAuthorizationAsync(postRoleActionAuthorizationRequest);
    }

    [HttpPut]
    [Route("PutRoleActionAuthorization")]
    [MustHavePermission(SubMenuKey: "RoleActionAuthorization", Controller: "RoleActionAuthorization", Action: "PutRoleActionAuthorization")]
    public async Task<CommonResponse<SaveRoleActionAuthorizationResponse>> PutRoleActionAuthorization([FromBody] SaveRoleActionAuthorizationRequest putRoleActionAuthorizationRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<SaveRoleActionAuthorizationResponse>.CreateWarningResponse(message: errorMessage);
        }
        return await _roleActionAuthorizationService.EditRoleActionAuthorizationAsync(putRoleActionAuthorizationRequest);
    }

    [HttpDelete]
    [Route("DeleteRoleActionAuthorization/{id}")]
    [MustHavePermission(SubMenuKey: "RoleActionAuthorization", Controller: "RoleActionAuthorization", Action: "DeleteRoleActionAuthorization")]
    public async Task<CommonResponse<long>> DeleteRoleActionAuthorization(long? id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _roleActionAuthorizationService.DeleteRoleActionAuthorizationAsync(Convert.ToInt64(id));
    }

    /*****************************

       RoleActionAuthorization Related Action End

    *****************************/
}
