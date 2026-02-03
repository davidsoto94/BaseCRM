using BaseCRM.Entities;
using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController (AccountService accountService,
    UserManager<ApplicationUser> userManager,
    IStringLocalizer<IdentityErrorMessages> localizer) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

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
            return Unauthorized(_localizer["UnauthorizedAccess"].Value);
        }
        // Placeholder for getting users logic
        return Ok("Get users endpoint");
    }
}
