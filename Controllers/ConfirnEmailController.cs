using BaseCRM.DTOs;
using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ConfirmEmailController (AccountService accountService, IStringLocalizer<IdentityErrorMessages> localizer): ControllerBase
{
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

    [HttpPost]
    public IActionResult Post([FromBody] ConfirmEmailDto confirmEmailDto)
    {
        var result = accountService.ConfirmEmail(confirmEmailDto.UserId, confirmEmailDto.Token).Result;
        if (result)
        {
            return Ok(_localizer["EmailConfirmed"].Value);
        }
        return BadRequest(new { _localizer["DefaultError"].Value } );
    }
}
