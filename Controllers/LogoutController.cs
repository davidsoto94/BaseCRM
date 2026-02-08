using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]

public class LogoutController (AccountService accountService
    , IStringLocalizer<IdentityErrorMessages> localizer) : ControllerBase
{
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

    [Authorize]
    [HttpPost()]
    public async Task<IActionResult> Post()
    {
        // Get refresh token from httpOnly cookie
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new[] { _localizer["InvalidCredentials"].Value });
        }

        var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var result = await accountService.RevokeRefreshToken(refreshToken, ipAddress);

        if (!result.Success)
            return BadRequest(new[] { _localizer[result.Error ?? "InvalidCredentials"].Value });

        // Clear the refresh token cookie
        Response.Cookies.Delete("refreshToken");

        return Ok(new { message = "Logout successful" });
    }
}
