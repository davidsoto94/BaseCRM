using BaseRMS.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace BaseRMS.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[ExcludeFromCodeCoverage]

public class LogoutController (AccountService accountService) : ControllerBase
{

    [HttpPost()]
    public async Task<IActionResult> Post()
    {
        // Get refresh token from httpOnly cookie
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Ok(new { message = "Logout successful" });
        }
        Response.Cookies.Delete("refreshToken");
        var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        await accountService.RevokeRefreshToken(refreshToken, ipAddress);
        return Ok(new { message = "Logout successful" });
    }
}
