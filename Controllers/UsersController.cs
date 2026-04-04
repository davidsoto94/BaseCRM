using BaseRMS.DTOs;
using BaseRMS.Entities;
using BaseRMS.Extensions;
using BaseRMS.Localization;
using BaseRMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace BaseRMS.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
[ExcludeFromCodeCoverage]
public class UsersController(AccountService accountService, 
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
    [HasPermission(Permissions.User.List)]
    public async Task<IActionResult> Get()
    {
        var users = await _userManager.Users.Select(s => new
            {
                s.Id,
                s.Name,
                s.LastName,
                s.Email,
                s.EmailConfirmed
            }).ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.User.View)]
    public async Task<IActionResult> Get(string id)
    {
        var user = await _userManager.Users.Include(i => i.TrustedDevices)
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
        if(user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    [HasPermission(Permissions.User.Create)]
    public async Task<IActionResult> Post(RegisterDTO model)
    {

        await accountService.RegisterNewUser(model, User);
        return Ok(_localizer["SuccessfulRegistration"].Value);
    }

    [HttpPost("{id}/disable")]
    [HasPermission(Permissions.User.Edit)]
    public async Task<IActionResult> Disable(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.LockoutEnd = DateTimeOffset.MaxValue;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    [HttpPost("{id}/enable")]
    [HasPermission(Permissions.User.Edit)]
    public async Task<IActionResult> Enable(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.LockoutEnd = null;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

}
