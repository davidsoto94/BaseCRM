using BaseCRM.Configurations;
using BaseCRM.DTOs;
using BaseCRM.Entities;
using BaseCRM.Enums;
using BaseCRM.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Web;

namespace BaseCRM.Services;

public class AccountService (
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IEmailSender emailSender,
    EmailTemplateService emailTemplateService,
    IdentityErrorLocalizerService identityErrorLocalizer,
    JWTTokenService jwtTokenService,
    IHttpContextAccessor httpContextAccessor,
    IStringLocalizer<EmailTemplates> emailLocalizer)
{

    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly EmailTemplateService _emailTemplateService = emailTemplateService;
    private readonly IdentityErrorLocalizerService _identityErrorLocalizer = identityErrorLocalizer;
    private readonly IStringLocalizer<EmailTemplates> _emailLocalizer = emailLocalizer;
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is required");


    public async Task<(bool Success, IEnumerable<string>? Errors)> ResetPasswordAsync(string email, string resetCode, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return (true, null);
        }

        // Use the UserManager.ResetPasswordAsync method to validate the token and update the password
        var result = await _userManager.ResetPasswordAsync(user, resetCode, newPassword);
        if (result.Succeeded)
        {
            // Optional: Invalidate any other existing tokens for enhanced security.
            await _userManager.UpdateSecurityStampAsync(user);

            return (true, null);
        }
        var localizedErrors = _identityErrorLocalizer.LocalizeErrors(result.Errors ?? []);
        return (false, localizedErrors);
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return (true, null);
        }

        // Generate a password reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var urlEncodedToken = HttpUtility.UrlEncode(token);
        var resetPasswordUrl = $"{Environment.GetEnvironmentVariable(Constants.ClientUrl)}/reset-password?email={email}&token={urlEncodedToken}";

        // Get localized email body
        var emailBody = await _emailTemplateService.GetResetPasswordEmailHtmlAsync(resetPasswordUrl, DateTime.Now.Year);
        var emailSubject = _emailLocalizer["EmailSubjectResetPassword"].Value;

        // Send the email with localized content
        await emailSender.SendEmailAsync(email, emailSubject, emailBody);
        return (true, null);
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> RegisterNewUser(RegisterDTO newUser, ApplicationUser requestUser)
    {
        var user = await _userManager.FindByEmailAsync(newUser.Email);
        user = new ApplicationUser
        {
            UserName = newUser.Email,
            Name = newUser.Name,
            LastName = newUser.LastName,
            Email = newUser.Email,
        };        

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            var localizedErrors = _identityErrorLocalizer.LocalizeErrors(result.Errors ?? []);
            return (false, localizedErrors);
        }

        newUser.roles = await RolesToBeAdded(newUser.roles, requestUser);
        var currentRoles = _roleManager.Roles.Where(w => newUser.roles.Contains(w.Name!)).Select(s => s.Name!).ToList();
        foreach (var role in currentRoles)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        await SendConfirmationEmail(user);
        return (true, null);
    }

    public async Task<(bool Success, string? AccessToken, string? RefreshToken, string? Error)> RefreshToken(string refreshToken, string ipAddress)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens
                .Any(t => t.Token == refreshToken));

        if (user == null)
            return (false, null, null, "Invalid token");

        var token = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

        if (token == null)
            return (false, null, null, "Invalid token");

        // If token is revoked, reject the request
        if (token.Revoked != null)
            return (false, null, null, "Token has been revoked");

        // If token is expired, replace it with a new one
        if (token.IsExpired)
        {
            var newRefreshToken = jwtTokenService.GenerateRefreshToken(ipAddress);
            token.ReplacedByToken = newRefreshToken.Token;
            token.Revoked = DateTime.Now;
            await _userManager.UpdateAsync(user);

            var newAccessToken = await jwtTokenService.GenerateJwtToken(user);
            return (true, newAccessToken, newRefreshToken.Token, null);
        }

        // Token is valid, generate new tokens
        var newAccessTokenValid = await jwtTokenService.GenerateJwtToken(user);

        return (true, newAccessTokenValid, null, null);
    }

    private async Task<List<string>> RolesToBeAdded(List<string> rolesAttempted, ApplicationUser requestUser)
    {
        var userRoles = (await _userManager.GetRolesAsync(requestUser)).ToList();
        return rolesAttempted.Where(w => userRoles.Contains(w)).ToList();
    }

    public async Task<bool> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null) return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> IsAuthorizedToAddNewUser(ApplicationUser? requestUser)
    {
        if (requestUser is null)
        {
            return false;
        }

        var permitions = await UserPermitions(requestUser);
        if (permitions.Contains(PermissionEnum.AddUser))
        {
            return true;
        }
        return false;
    }
    public async Task<bool> IsAuthorizedToViewUsers(ApplicationUser? requestUser)
    {
        if (requestUser is null)
        {
            return false;
        }

        var permitions = await UserPermitions(requestUser);
        if (permitions.Contains(PermissionEnum.ViewUser))
        {
            return true;
        }
        return false;
    }

    private async Task<List<PermissionEnum>> UserPermitions(ApplicationUser requestUser)
    {
        var userRoles = (await _userManager.GetRolesAsync(requestUser)).ToList();
        return (await _roleManager.Roles
            .Where(w => w.Name != null && userRoles.Contains(w.Name))
            .Select(s => s.Permitions)
            .ToListAsync())
            .SelectMany(s => s)
            .ToList();
    }

    public async Task SendConfirmationEmail(ApplicationUser user)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var urlEncodedCode = HttpUtility.UrlEncode(code);
        var confirmationUrl = $"{Environment.GetEnvironmentVariable(Constants.ClientUrl)}/confirm-email?userId={user.Id}&token={urlEncodedCode}";

        // Get localized email body
        var emailBody = await _emailTemplateService.GetConfirmationEmailHtmlAsync(user.Name ?? "User", confirmationUrl, DateTime.Now.Year);
        var emailSubject = _emailLocalizer["EmailSubjectConfirmEmail"].Value;

        // Send the confirmation email
        await emailSender.SendEmailAsync(user.Email!, emailSubject, emailBody);
    }

    public async Task<string?> GetAccessTokenWithRefreshToken(ApplicationUser user)
    {
        if (!_httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
        {
            var ipAddress = _httpContext.Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            var newToken = jwtTokenService.GenerateRefreshToken(ipAddress);
            _httpContext.Response.Cookies.Append("refreshToken", newToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
            user.RefreshTokens.Add(newToken);
            await _userManager.UpdateAsync(user);
        }
        var accessToken = await jwtTokenService.GenerateJwtToken(user);
        return accessToken;

    }

    public async Task<(bool Success, string? Error)> RevokeRefreshToken(string refreshToken, string ipAddress)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens
                .Any(t => t.Token == refreshToken));

        if (user == null)
            return (false, "Invalid token");

        var token = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

        if (token == null)
            return (false, "Invalid token");

        // If token is already revoked, return success
        if (token.Revoked != null)
            return (true, null);

        // Revoke the token
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var localizedErrors = _identityErrorLocalizer.LocalizeErrors(result.Errors ?? []);
            return (false, string.Join(", ", localizedErrors ?? []));
        }

        return (true, null);
    }
}
