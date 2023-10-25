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
public class ActionAuthorizationController : Controller
{
    private readonly ILogger<ActionAuthorizationController> _logger;

    private readonly IActionAuthorizationService _ActionAuthorizationService;
    private readonly ISessionManager _sessionManager;
    public ActionAuthorizationController(ILogger<ActionAuthorizationController> logger,
        IActionAuthorizationService ActionAuthorizationService,
        ISessionManager sessionManager
        )
    {
        _logger = logger;
        _ActionAuthorizationService = ActionAuthorizationService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "ActionAuthorization", Controller: "ActionAuthorization", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/AccessControl/Views/ActionAuthorization/Index.cshtml");
    }

    [Route("GetPermittedActionAuthorization")]
    [MustHavePermission(SubMenuKey: "ActionAuthorization", Controller: "ActionAuthorization", Action: "Index")]
    public async Task<CommonResponse<ActionPermission>> GetPermittedSubActionAuthorization()
    {
        var actions = new[] { "GetActionAuthorizationList", "PostActionAuthorization", "PutActionAuthorization", "DeleteActionAuthorization" };
        var permissions = await _sessionManager.CheckPermissionsAsync("ActionAuthorization", actions);

        ActionPermission actionPermission = new ActionPermission
        {
            View = permissions.GetValueOrDefault("GetActionAuthorizationList", false),
            Create = permissions.GetValueOrDefault("PostActionAuthorization", false),
            Edit = permissions.GetValueOrDefault("PutActionAuthorization", false),
            Delete = permissions.GetValueOrDefault("DeleteActionAuthorization", false)
        };
        return CommonResponse<ActionPermission>.CreateHappyResponse(actionPermission);

    }

    /*****************************
        
        ActionAuthorization Related Action Start

    *****************************/

    [Route("GetActionAuthorizationList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "ActionAuthorization", Controller: "ActionAuthorization", Action: "GetActionAuthorizationList")]
    public async Task<CommonDataTableResponse<IEnumerable<ActionAuthorizationVM>>> GetActionAuthorizationList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _ActionAuthorizationService.GetActionAuthorizationListAsync(getDataTableRequest);
    }

    [Route("GetActionAuthorizationListForSelect")]
    [HttpGet]
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetActionAuthorizationListForSelect()
    {
        return await _ActionAuthorizationService.GetActionAuthorizationListForSelectAsync();
    }
    [HttpGet]
    [Route("GetActionAuthorization/{id}")]
    [MustHavePermission(SubMenuKey: "ActionAuthorization", Controller: "ActionAuthorization", Action: "GetActionAuthorization")]
    public async Task<CommonResponse<ActionAuthorizationVM>> GetActionAuthorization(long id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<ActionAuthorizationVM>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _ActionAuthorizationService.GetActionAuthorizationAsync(id);
    }

    [HttpPost]
    [Route("PostActionAuthorization")]
    [MustHavePermission(SubMenuKey: "ActionAuthorization", Controller: "ActionAuthorization", Action: "PostActionAuthorization")]
    public async Task<CommonResponse<SaveActionAuthorizationResponse>> PostActionAuthorization([FromBody] SaveActionAuthorizationRequest postActionAuthorizationRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<SaveActionAuthorizationResponse>.CreateWarningResponse(message: errorMessage);
        }
        return await _ActionAuthorizationService.AddActionAuthorizationAsync(postActionAuthorizationRequest);
    }

    [HttpPut]
    [Route("PutActionAuthorization")]
    [MustHavePermission(SubMenuKey: "ActionAuthorization", Controller: "ActionAuthorization", Action: "PutActionAuthorization")]
    public async Task<CommonResponse<SaveActionAuthorizationResponse>> PutActionAuthorization([FromBody] SaveActionAuthorizationRequest putActionAuthorizationRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<SaveActionAuthorizationResponse>.CreateWarningResponse(message: errorMessage);
        }
        return await _ActionAuthorizationService.EditActionAuthorizationAsync(putActionAuthorizationRequest);
    }

    [HttpDelete]
    [Route("DeleteActionAuthorization/{id}")]
    [MustHavePermission(SubMenuKey: "ActionAuthorization", Controller: "ActionAuthorization", Action: "DeleteActionAuthorization")]
    public async Task<CommonResponse<long>> DeleteActionAuthorization(long? id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _ActionAuthorizationService.DeleteActionAuthorizationAsync(Convert.ToInt64(id));
    }

    /*****************************

       ActionAuthorization Related Action End

    *****************************/
}
