using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace XoomCore.Services.SessionControl;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MustHavePermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    public string _SubMenuKey { get; }
    public string _Controller { get; }
    public string _Action { get; }

    public MustHavePermissionAttribute(CoreController controllerName, CoreAction action)
    {
        _Controller = controllerName.ToString();
        _Action = action.ToString();
    }
    public MustHavePermissionAttribute(string SubMenuKey, string Controller, string Action)
    {
        this._SubMenuKey = SubMenuKey;
        this._Controller = Controller;
        this._Action = Action;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var _sessionManager = context.HttpContext.RequestServices.GetRequiredService<ISessionManager>();

        bool hasPermission = await _sessionManager.CheckPermissionAsync(_Controller, _Action);

        if (!hasPermission)
        {
            // Redirect to the SignIn page when permission is not granted
            context.Result = new RedirectToRouteResult(new
            {
                controller = "SignIn",
                action = "Index"
            });
        }
        _sessionManager.SetActiveMenuItem(_SubMenuKey);
    }
}
