using BaseCRM.DTOs;
using BaseCRM.Entities;
using BaseCRM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LoginController (UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , JWTTokenService jwtTokenService): ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;


    [HttpPost]
    public async Task<IActionResult> Post(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user == null)
        {
            return BadRequest("Invalid credentials");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(
            user, loginDTO.Password, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            var token = await jwtTokenService.GenerateJwtToken(user);
            return Ok(new { token });
        }
        return BadRequest("Invalid credentials");
    }
}
