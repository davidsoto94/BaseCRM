using BaseCRM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ResetPassword (AccountService accountService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ResetPasswordRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, errors) = await accountService.ResetPasswordAsync(model.Email, model.ResetCode, model.NewPassword);

        if (success) {
            return Ok(new { Message = "Password has been reset successfully." });
        }

        return BadRequest(new { Errors = errors });

    }
}
