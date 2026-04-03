using BaseRMS.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        var permissionsClaim = user.Claims.FirstOrDefault(c => c.Type == Constants.PermissionsClaimType)?.Value;

        if (String.IsNullOrEmpty(permissionsClaim))
        {
            context.Result = new ForbidResult();
            return;
        }

        var permission = JsonSerializer.Deserialize<List<string>>(permissionsClaim);
        var hasPermission = permission != null && permission.Contains(_permission);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}