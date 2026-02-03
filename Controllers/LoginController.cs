using BaseCRM.DTOs;
using BaseCRM.Entities;
using BaseCRM.Localization;
using BaseCRM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BaseCRM.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LoginController (UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , JWTTokenService jwtTokenService
    , IStringLocalizer<IdentityErrorMessages> localizer): ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IStringLocalizer<IdentityErrorMessages> _localizer = localizer;


    [HttpPost]
    public async Task<IActionResult> Post(LoginDTO loginDTO)
    {
        var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
        var culture = rqf.RequestCulture.Culture.Name; // e.g., "en-US"

        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user == null)
        {
            return BadRequest(new[] { _localizer["InvalidCredentials"].Value } );
        }
        var result = await _signInManager.CheckPasswordSignInAsync(
            user, loginDTO.Password, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            var token = await jwtTokenService.GenerateJwtToken(user);
            return Ok(new { token });
        }
        return BadRequest(new[] { _localizer["InvalidCredentials"].Value } );
    }
}
