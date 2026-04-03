using BaseRMS.Entities;
using BaseRMS.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BaseRMS.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[ExcludeFromCodeCoverage]
public class UsersController (UserManager<ApplicationUser> userManager) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    /// <summary>
    /// Get a list of all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [HasPermission(Permissions.User.List)]
    [Authorize]
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
}
