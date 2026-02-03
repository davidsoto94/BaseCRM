using BaseCRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ConfirnEmailController (AccountService accountService): ControllerBase
{
    [HttpPost]
    public IActionResult Post(string userId, string token)
    {
        var result = accountService.ConfirmEmail(userId, token).Result;
        if (result)
        {
            return Ok(new { Message = "Email confirmed successfully." });
        }
        return BadRequest(new { Message = "Email confirmation failed." });
    }
}
