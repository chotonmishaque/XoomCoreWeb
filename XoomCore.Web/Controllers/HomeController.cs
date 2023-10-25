using Microsoft.AspNetCore.Mvc;
using XoomCore.Services.SessionControl;

namespace XoomCore.Web.Controllers;

[MustHaveAuthorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISessionManager _sessionManager;
    public HomeController(ILogger<HomeController> logger, ISessionManager sessionManager)
    {
        _logger = logger;
        _sessionManager = sessionManager;

    }
    [Route("index")]
    [MustHavePermission(SubMenuKey: "Home", Controller: "Home", Action: "Index")]
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    [Route("GetUser")]
    public ActionResult GetUser()
    {
        return Ok(new { error = false, MenuList = _sessionManager.Current.SubMenuAuthorizedList, UserId = _sessionManager.Current.UserId });
    }

}