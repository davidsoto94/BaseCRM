using BaseRMS.Localization;
using BaseRMS.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace BaseRMS.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[ExcludeFromCodeCoverage]
public class PasswordController (AccountService accountService
    , IStringLocalizer<IdentityErrorMessages> localizer) : ControllerBase
{
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

    [HttpPost("reset")]
    public async Task<IActionResult> Post([FromBody] ResetPasswordRequest model)
    {
        await accountService.ResetPasswordAsync(model);
        return Ok(_localizer["SuccessfullPasswordReset"]);
    }

    [HttpPost("forgot")]
    public async Task<IActionResult> Post([FromBody] ForgotPasswordRequest model)
    {
        await accountService.ForgotPassword(model.Email);
        return Ok(_localizer["SuccessfulForgotPassword"].Value);
    }
}
