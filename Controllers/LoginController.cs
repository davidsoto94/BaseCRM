using BaseCRM.DTOs;
using BaseCRM.Entities;
using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LoginController (UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , JWTTokenService jwtTokenService
    , IStringLocalizer<IdentityErrorMessages> localizer): ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;


    [HttpPost]
    public async Task<IActionResult> Post(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user == null)
        {
            return BadRequest(new[] { _localizer["InvalidCredentials"].Value } );
        }
        var result = await _signInManager.CheckPasswordSignInAsync(
            user, loginDTO.Password, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            var accessToken = await jwtTokenService.GenerateJwtToken(user);
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            var refreshToken = jwtTokenService.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                //Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = refreshToken.Expires
            });

            return Ok(new { accessToken });
        }
        return BadRequest(new[] { _localizer["InvalidCredentials"].Value } );
    }
}
