using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Glimmer.Creator.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class RequireAuthenticationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.Session.GetString("UserId");
        
        if (string.IsNullOrEmpty(userId))
        {
            context.HttpContext.Session.SetString("ReturnUrl", context.HttpContext.Request.Path);
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }
        
        base.OnActionExecuting(context);
    }
}
