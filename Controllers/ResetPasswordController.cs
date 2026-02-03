using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResetPasswordController (AccountService accountService
    , IStringLocalizer<IdentityErrorMessages> localizer) : ControllerBase
{

    
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ResetPasswordRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, errors) = await accountService.ResetPasswordAsync(model.Email, model.ResetCode, model.NewPassword);

        if (success) {
            return Ok(_localizer["SuccessfullPasswordReset"]);
        }

        return BadRequest(errors);

    }
}
