using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ConfirnEmailController (AccountService accountService, IStringLocalizer<IdentityErrorMessages> localizer): ControllerBase
{
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

    [HttpPost]
    public IActionResult Post(string userId, string token)
    {
        var result = accountService.ConfirmEmail(userId, token).Result;
        if (result)
        {
            return Ok(_localizer["EmailConfirmed"].Value);
        }
        return BadRequest(new { _localizer["DefaultError"].Value } );
    }
}
