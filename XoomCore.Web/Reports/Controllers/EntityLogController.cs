using Microsoft.AspNetCore.Mvc;
using XoomCore.Application.RequestModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Application.ViewModels.Report;
using XoomCore.Services.Contracts.Report;
using XoomCore.Services.SessionControl;

namespace XoomCore.Web.Reports.Controllers;

[MustHaveAuthorize]
[Route("[controller]")]
public class EntityLogController : Controller
{

    private readonly IEntityLogService _EntityLogService;
    private readonly ISessionManager _sessionManager;
    public EntityLogController(
        IEntityLogService EntityLogService,
        ISessionManager sessionManager
        )
    {
        _EntityLogService = EntityLogService;
        _sessionManager = sessionManager;
    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "EntityLog", Controller: "EntityLog", Action: "Index")]
    public IActionResult Index()
    {
        return View("~/Reports/Views/EntityLog/Index.cshtml");
    }

    [Route("GetEntityLogList")]
    [HttpGet]
    [MustHavePermission(SubMenuKey: "EntityLog", Controller: "EntityLog", Action: "GetEntityLogList")]
    public async Task<CommonDataTableResponse<IEnumerable<EntityLogVM>>> GetEntityLogList([FromQuery] GetDataTableRequest getDataTableRequest)
    {
        return await _EntityLogService.GetEntityLogListAsync(getDataTableRequest);
    }
}
