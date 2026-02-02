using BaseCRM.DTOs;
using BaseCRM.Entities;
using BaseCRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RegisterController(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    AccountService accountService) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var user = await _userManager.GetUserAsync(User);
        if(user is null)
        {
            return Unauthorized();
        }
        var userRoles = user is null ? [] : (await _userManager.GetRolesAsync(user)).ToList();
        if (userRoles.Contains("Admin"))
        {
            userRoles = [.. _roleManager.Roles.Where(w => w.Name != null).Select(s => s.Name!)];
        }
        return Ok(userRoles);
    }
    [HttpPost]
    public async Task<IActionResult> Post(RegisterDTO model)
    {
        var result = await accountService.RegisterNewUser(model);
        if (result.Success)
        {
            return Ok();
        }
        return BadRequest(new { errors = result.Errors });
    }


}
