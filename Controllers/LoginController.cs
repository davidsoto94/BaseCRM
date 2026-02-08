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
    , AccountService accountService
    , DeviceTrustService deviceTrustService
    , IStringLocalizer<IdentityErrorMessages> localizer): ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly DeviceTrustService _deviceTrustService = deviceTrustService;
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
            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var deviceFingerprint = _deviceTrustService.GenerateDeviceFingerprint();
            var isDeviceTrusted = await _deviceTrustService.IsDeviceTrusted(user, deviceFingerprint);

            // RequireSetupMfa: true = user must SET UP MFA (doesn't have it yet)
            // MfaRequired: true = user has MFA enabled AND device is not trusted
            var mfaRequired = isTwoFactorEnabled && !isDeviceTrusted;

            string? accessToken = null;
            string? tempToken = null;

            if (mfaRequired || !isTwoFactorEnabled)
            {
                // Generate scoped token for MFA verification only
                tempToken = await jwtTokenService.GenerateJwtToken(user, scope: "mfa_verification");
            }
            else
            {
                // Generate full access token
                accessToken = await accountService.GetAccessTokenWithRefreshToken(user);
            }

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                TempToken = tempToken,
                RequireSetupMfa = !isTwoFactorEnabled,
                MfaRequired = mfaRequired
            };

            return Ok(response);
        }
        return BadRequest(new[] { _localizer["InvalidCredentials"].Value } );
    }
}
