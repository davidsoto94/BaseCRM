using BaseCRM.Entities;
using BaseCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController (AccountService accountService,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    /// <summary>
    /// Get a list of all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var user = await _userManager.GetUserAsync(User);
        var isAuthorizedToViewUsers = await accountService.IsAuthorizedToViewUsers(user);
        if (!isAuthorizedToViewUsers)
        {
            return Unauthorized();
        }
        var users = 

        return Ok();

        // Placeholder for getting users logic
        return Ok(new { Message = "Get users endpoint" });
    }
}
