using BaseCRM.DTOs;
using BaseCRM.Entities;
using BaseCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaseCRM.Controllers;

[Route("api/v1/mfa")]
[ApiController]
[Authorize]
public class MfaController (UserManager<ApplicationUser> userManager, 
    MfaService mfaService, 
    DeviceTrustService deviceTrustService,
    AccountService accountService) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly MfaService _mfaService = mfaService;
    private readonly DeviceTrustService _deviceTrustService = deviceTrustService;
    private readonly AccountService _accountService = accountService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        return Ok(new { enabled = isTwoFactorEnabled });
    }

    [HttpPost("setup")]
    public async Task<IActionResult> Setup()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var (success, setup, errors) = await _mfaService.GenerateMfaSetupAsync(user);
        if (!success)
        {
            return BadRequest(errors);
        }

        return Ok(setup);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] MfaVerifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest("Code is required");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var (success, errors) = await _mfaService.VerifyAndEnableMfaAsync(user, request.Code);
        if (!success)
        {
            return BadRequest(errors);
        }

        var (codesSuccess, codesErrors) = await _mfaService.GenerateRecoveryCodesAsync(user);
        if (!codesSuccess)
        {
            return BadRequest(codesErrors);
        }

        var recoveryCodes = await _mfaService.GetRecoveryCodesAsync(user);
        return Created(string.Empty, new { enabled = true, recoveryCodes = recoveryCodes.ToList() });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var (success, errors) = await _mfaService.DisableMfaAsync(user);
        if (!success)
        {
            return BadRequest(errors);
        }

        return NoContent();
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] MfaVerifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest("Code is required");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            return BadRequest("MFA is not enabled");
        }

        var (success, errors) = await _mfaService.VerifyMfaCodeAsync(user, request.Code);
        if (!success)
        {
            return BadRequest(errors);
        }

        var accessToken = await _accountService.GetAccessTokenWithRefreshToken(user);
        return Ok(new { verified = true, accessToken = accessToken });
    }

    [HttpPost("verify/trust-device")]
    public async Task<IActionResult> VerifyAndTrustDevice([FromBody] MfaVerifyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest("Code is required");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            return BadRequest("MFA is not enabled");
        }

        var (success, errors) = await _mfaService.VerifyMfaCodeAsync(user, request.Code);
        if (!success)
        {
            return BadRequest(errors);
        }
        // Trust this device after successful MFA verification
        var deviceFingerprint = _deviceTrustService.GenerateDeviceFingerprint();
        var deviceName = _deviceTrustService.ExtractDeviceName();
        _deviceTrustService.AddTrustedDevice(user, deviceFingerprint, deviceName);
        await _userManager.UpdateAsync(user);

        // Generate and return full access token
        var accessToken = await _accountService.GetAccessTokenWithRefreshToken(user);

        return Ok(new { verified = true, deviceTrusted = true, accessToken = accessToken });
    }
}
