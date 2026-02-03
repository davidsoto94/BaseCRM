using BaseCRM.DTOs;
using BaseCRM.Entities;
using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RegisterController(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    AccountService accountService,
    IStringLocalizer<IdentityErrorMessages> localizer) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var user = await _userManager.GetUserAsync(User);
        var isAuthorizedToAdd = accountService.IsAuthorizedToAddNewUser(user).Result;
        if (!isAuthorizedToAdd)
        {
            return Unauthorized(_localizer["UnauthorizedAccess"].Value);
        }
        var userRoles = user is null ? [] : (await _userManager.GetRolesAsync(user)).ToList();
        if (userRoles.Contains("Admin"))
        {
            userRoles = [.. _roleManager.Roles.Where(w => w.Name != null).Select(s => s.Name!)];
        }
        return Ok(userRoles);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(RegisterDTO model)
    {
        var user = await _userManager.GetUserAsync(User);
        var isAuthorizedToAdd = accountService.IsAuthorizedToAddNewUser(user).Result;
        if (!isAuthorizedToAdd)
        {
            return Unauthorized(_localizer["UnauthorizedAccess"].Value);
        }
        var result = await accountService.RegisterNewUser(model, user!);
        if (result.Success)
        {
            return Ok(_localizer["SuccessfulRegistration"].Value);
        }        
        return BadRequest(result.Errors);
    }
}