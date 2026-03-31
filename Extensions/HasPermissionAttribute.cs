using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaseRMS.Extensions;

public class HasPermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _permission;

    public HasPermissionAttribute(string permission)
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // 🔒 Ensure user is authenticated (extra safety)
        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasPermission = user.Claims
            .Where(c => c.Type == "Permission")
            .Any(c => c.Value == _permission);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}