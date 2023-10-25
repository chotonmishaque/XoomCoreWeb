using Microsoft.AspNetCore.Mvc;
using XoomCore.Application.RequestModels;
using XoomCore.Application.ResponseModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Services.Contracts.AccessControl;
using XoomCore.Services.SessionControl;

namespace XoomCore.Web.Authentication.Controllers
{
    public class SignInController : Controller
    {

        private readonly ILogger<SignInController> _logger;
        private readonly IUserService _userService;
        private readonly ISessionManager _sessionManager;
        public SignInController(ILogger<SignInController> logger,
            IUserService userService, ISessionManager sessionManager)
        {
            _logger = logger;
            _userService = userService;
            _sessionManager = sessionManager;
        }
        [Route("")]
        [Route("sign_in")]
        public async Task<IActionResult> Index()
        {
            return View("~/Authentication/Views/SignIn/Index.cshtml");
        }
        [Route("sign_out")]
        public ActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/sign_in");
        }
        [Route("PostUserSignIn")]
        [HttpPost]
        public async Task<CommonResponse<SignInResponse>> PostUserSignIn([FromBody] SignInRequest signInRequest)
        {
            _logger.LogInformation("PostUserSignIn by : {email}", signInRequest.Email);
            if (!ModelState.IsValid)
            {
                string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                return CommonResponse<SignInResponse>.CreateWarningResponse(message: errorMessage);
            }
            return await _userService.CreateSessionAsync(signInRequest);
        }
    }
}
