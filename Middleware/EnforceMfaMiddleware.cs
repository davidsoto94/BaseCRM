using BaseCRM.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BaseCRM.Middleware;

/// <summary>
/// Middleware to enforce MFA requirement on protected endpoints.
/// Users without MFA enabled are restricted from accessing most endpoints.
/// Allowed endpoints without MFA: login, register, password reset, mfa setup/verify
/// </summary>
public class EnforceMfaMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EnforceMfaMiddleware> _logger;

    // Paths that don't require MFA verification
    private static readonly HashSet<string> ExemptPaths = new()
    {
        "/api/v1/login",
        "/api/v1/forgotpassword",
        "/api/v1/resetpassword",
        "/api/v1/confirmEmail",
        "/api/v1/resendconfirmationemail",
        "/api/v1/mfa",
        "/api/v1/mfa/setup",
        "/api/v1/mfa/verify",
        "/api/v1/refreshToken",
    };

    public EnforceMfaMiddleware(RequestDelegate next, ILogger<EnforceMfaMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
    {
        var user = context.User;

        // Only enforce MFA for authenticated users
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var requestPath = context.Request.Path.Value?.ToLower() ?? string.Empty;

            // Skip exempt paths
            if (!IsExemptPath(requestPath))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (!string.IsNullOrEmpty(userId))
                {
                    var appUser = await userManager.FindByIdAsync(userId);
                    if (appUser != null)
                    {
                        var isMfaEnabled = await userManager.GetTwoFactorEnabledAsync(appUser);
                        
                        // If user doesn't have MFA enabled, deny access
                        if (!isMfaEnabled)
                        {
                            _logger.LogWarning($"User {userId} attempted to access {requestPath} without MFA enabled");
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsJsonAsync(new 
                            { 
                                error = "MFA is required to access this endpoint",
                                requiresMfa = true 
                            });
                            return;
                        }
                    }
                }
            }
        }

        await _next(context);
    }

    private static bool IsExemptPath(string path)
    {
        // Check exact matches
        if (ExemptPaths.Contains(path))
            return true;

        // Check if path starts with exempt paths (to allow query strings, etc.)
        foreach (var exemptPath in ExemptPaths)
        {
            if (path.StartsWith(exemptPath, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
