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
public class SubMenuController : Controller
{
    private readonly ILogger<SubMenuController> _logger;

    private readonly ISubMenuService _subMenuService;
    private readonly ISessionManager _sessionManager;
    public SubMenuController(ILogger<SubMenuController> logger,
        ISubMenuService subMenuService,
        ISessionManager sessionManager
        )
    {
        _logger = logger;
        _subMenuService = subMenuService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "SubMenu", Controller: "SubMenu", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/AccessControl/Views/SubMenu/Index.cshtml");
    }

    [Route("GetPermittedSubMenu")]
    [MustHavePermission(SubMenuKey: "SubMenu", Controller: "SubMenu", Action: "Index")]
    public async Task<CommonResponse<ActionPermission>> GetPermittedSubSubMenu()
    {
        var actions = new[] { "GetSubMenuList", "PostSubMenu", "PutSubMenu", "DeleteSubMenu" };
        var permissions = await _sessionManager.CheckPermissionsAsync("SubMenu", actions);

        ActionPermission actionPermission = new ActionPermission
        {
            View = permissions.GetValueOrDefault("GetSubMenuList", false),
            Create = permissions.GetValueOrDefault("PostSubMenu", false),
            Edit = permissions.GetValueOrDefault("PutSubMenu", false),
            Delete = permissions.GetValueOrDefault("DeleteSubMenu", false)
        };
        return CommonResponse<ActionPermission>.CreateHappyResponse(actionPermission);

    }

    /*****************************
        
        SubMenu Related Action Start

    *****************************/

    [Route("GetSubMenuList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "SubMenu", Controller: "SubMenu", Action: "GetSubMenuList")]
    public async Task<CommonDataTableResponse<IEnumerable<SubMenuVM>>> GetSubMenuList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _subMenuService.GetSubMenuListAsync(getDataTableRequest);
    }

    [Route("GetSubMenuListForSelect")]
    [HttpGet]
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetSubMenuListForSelect()
    {
        return await _subMenuService.GetSubMenuListForSelectAsync();
    }
    [HttpGet]
    [Route("GetSubMenu/{id}")]
    [MustHavePermission(SubMenuKey: "SubMenu", Controller: "SubMenu", Action: "GetSubMenu")]
    public async Task<CommonResponse<SubMenuVM>> GetSubMenu(long id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<SubMenuVM>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _subMenuService.GetSubMenuAsync(id);
    }

    [HttpPost]
    [Route("PostSubMenu")]
    [MustHavePermission(SubMenuKey: "SubMenu", Controller: "SubMenu", Action: "PostSubMenu")]
    public async Task<CommonResponse<long>> PostSubMenu([FromBody] SaveSubMenuRequest postSubMenuRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _subMenuService.AddSubMenuAsync(postSubMenuRequest);
    }

    [HttpPut]
    [Route("PutSubMenu")]
    [MustHavePermission(SubMenuKey: "SubMenu", Controller: "SubMenu", Action: "PutSubMenu")]
    public async Task<CommonResponse<long>> PutSubMenu([FromBody] SaveSubMenuRequest putSubMenuRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _subMenuService.EditSubMenuAsync(putSubMenuRequest);
    }

    [HttpDelete]
    [Route("DeleteSubMenu/{id}")]
    [MustHavePermission(SubMenuKey: "SubMenu", Controller: "SubMenu", Action: "DeleteSubMenu")]
    public async Task<CommonResponse<long>> DeleteSubMenu(long? id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _subMenuService.DeleteSubMenuAsync(Convert.ToInt64(id));
    }

    /*****************************

       SubMenu Related Action End

    *****************************/
}
