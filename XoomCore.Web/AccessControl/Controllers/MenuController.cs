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
public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;

    private readonly IMenuService _menuService;
    private readonly ISessionManager _sessionManager;
    public MenuController(ILogger<MenuController> logger,
        IMenuService menuService,
        IMenuService MenuService,
        ISessionManager sessionManager
        )
    {
        _logger = logger;
        _menuService = menuService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "Menu", Controller: "Menu", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/AccessControl/Views/Menu/Index.cshtml");
    }

    [Route("GetPermittedSubMenu")]
    [MustHavePermission(SubMenuKey: "Menu", Controller: "Menu", Action: "Index")]
    public async Task<CommonResponse<ActionPermission>> GetPermittedSubMenu()
    {
        var actions = new[] { "GetMenuList", "PostMenu", "PutMenu", "DeleteMenu" };
        var permissions = await _sessionManager.CheckPermissionsAsync("Menu", actions);

        ActionPermission actionPermission = new ActionPermission
        {
            View = permissions.GetValueOrDefault("GetMenuList", false),
            Create = permissions.GetValueOrDefault("PostMenu", false),
            Edit = permissions.GetValueOrDefault("PutMenu", false),
            Delete = permissions.GetValueOrDefault("DeleteMenu", false)
        };
        return CommonResponse<ActionPermission>.CreateHappyResponse(actionPermission);

    }

    /*****************************
        
        Menu Related Action Start

    *****************************/

    [Route("GetMenuList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "Menu", Controller: "Menu", Action: "GetMenuList")]
    public async Task<CommonDataTableResponse<IEnumerable<MenuVM>>> GetMenuList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _menuService.GetMenuListAsync(getDataTableRequest);
    }

    [Route("GetMenuListForSelect")]
    [HttpGet]
    public async Task<CommonResponse<IEnumerable<SelectOptionResponse>>> GetMenuListForSelect()
    {
        return await _menuService.GetMenuListForSelectAsync();
    }
    [HttpGet]
    [Route("GetMenu/{id}")]
    [MustHavePermission(SubMenuKey: "Menu", Controller: "Menu", Action: "GetMenu")]
    public async Task<CommonResponse<MenuVM>> GetMenu(long id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<MenuVM>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _menuService.GetMenuAsync(id);
    }

    [HttpPost]
    [Route("PostMenu")]
    [MustHavePermission(SubMenuKey: "Menu", Controller: "Menu", Action: "PostMenu")]
    public async Task<CommonResponse<long>> PostMenu([FromBody] SaveMenuRequest postMenuRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _menuService.AddMenuAsync(postMenuRequest);
    }

    [HttpPut]
    [Route("PutMenu")]
    [MustHavePermission(SubMenuKey: "Menu", Controller: "Menu", Action: "PutMenu")]
    public async Task<CommonResponse<long>> PutMenu([FromBody] SaveMenuRequest putMenuRequest)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<long>.CreateWarningResponse(message: errorMessage);
        }
        return await _menuService.EditMenuAsync(putMenuRequest);
    }

    [HttpDelete]
    [Route("DeleteMenu/{id}")]
    [MustHavePermission(SubMenuKey: "Menu", Controller: "Menu", Action: "DeleteMenu")]
    public async Task<CommonResponse<long>> DeleteMenu(long? id)
    {
        if (id == null || id <= 0)
        {
            return CommonResponse<long>.CreateWarningResponse(message: "Invalid request detected!");
        }
        return await _menuService.DeleteMenuAsync(Convert.ToInt64(id));
    }

    /*****************************

       Menu Related Action End

    *****************************/
}
