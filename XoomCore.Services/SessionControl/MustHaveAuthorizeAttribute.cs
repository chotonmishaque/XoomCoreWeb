using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Services.SessionControl;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MustHaveAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var _sessionManager = context.HttpContext.RequestServices.GetRequiredService<ISessionManager>();

        long userNo = _sessionManager.Current.UserId;
        if (userNo <= 0)
        {
            context.Result = new RedirectToRouteResult(new
            {
                controller = "SignIn",
                action = "Index"
            });
        }

        //UserEmailStatus userEmailStatus = SessionManager.Current.UserEmailStatus;
        //if (userEmailStatus == UserEmailStatus.NotVerified)
        //{
        //    context.Result = new RedirectToRouteResult(new
        //    {
        //        controller = "SignIn",
        //        action = "Index"
        //    });
        //}
    }
}