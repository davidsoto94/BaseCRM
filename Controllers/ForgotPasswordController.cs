using BaseCRM.Configurations;
using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Web;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ForgotPasswordController (
    AccountService accountService,
    IStringLocalizer<IdentityErrorMessages> localizer
    ) : ControllerBase
{

    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ForgotPasswordRequest model)
    {
        var result = await accountService.ForgotPassword(model.Email);
        if (result.Success)
        {
            return Ok(_localizer["SuccessfulForgotPassword"].Value);
        }
        return BadRequest(result.Errors);

    }

}
