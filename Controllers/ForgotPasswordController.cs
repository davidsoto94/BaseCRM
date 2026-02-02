using BaseCRM.Configurations;
using BaseCRM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ForgotPasswordController (
    AccountService accountService
    ) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ForgotPasswordRequest model)
    {
        var result = await accountService.ForgotPassword(model.Email);
        if (result.Success)
        {
            return Ok(new { Status = "Success", Message = "If an account with that email exists, a password reset link has been sent." });
        }
        return BadRequest(new { Errors = result.Errors });

    }

}
