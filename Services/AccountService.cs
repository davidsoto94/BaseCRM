using BaseCRM.Configurations;
using BaseCRM.DTOs;
using BaseCRM.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Web;

namespace BaseCRM.Services;

public class AccountService (
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IEmailSender emailSender)
{

    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<(bool Success, IEnumerable<IdentityError>? Errors)> ResetPasswordAsync(string email, string resetCode, string newPassword)
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
        return (false, result.Errors);
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return (true,null);
        }
        // Generate a password reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var urlEncodedToken = HttpUtility.UrlEncode(token);
        var resetPasswordUrl = $"{Environment.GetEnvironmentVariable(Constants.ClientUrl)}/reset-password?email={email}&token={urlEncodedToken}";

        // Load and populate the HTML email template
        var emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates/ResetPasswordEmail.html");
        var emailBody = await File.ReadAllTextAsync(emailTemplatePath);
        emailBody = emailBody.Replace("{RESET_PASSWORD_URL}", resetPasswordUrl);
        emailBody = emailBody.Replace("{CURRENT_YEAR}", DateTime.Now.Year.ToString());

        // Send the email with the styled HTML
        await emailSender.SendEmailAsync(email, "Reset Your Password", emailBody);
        return (true,null);
    }

    public async Task<(bool Success, IEnumerable<IdentityError>? Errors)> RegisterNewUser(RegisterDTO newUser)
    {

        var user = await _userManager.FindByEmailAsync(newUser.Email);

        if(user is not null && !user.EmailConfirmed)
        {
            await SendConfirmationEmail(user);
        }

        user = new ApplicationUser
        {
            Name = newUser.Name,
            LastName = newUser.LastName,
            Email = newUser.Email,
        };

        

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            return (false, result.Errors);
        }

        var currentRoles = _roleManager.Roles.Where(w => newUser.roles.Contains(w.Name!)).Select(s => s.Name!).ToList();
        foreach (var role in currentRoles)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        await SendConfirmationEmail(user);

        return (true, null);
    }

    private async Task SendConfirmationEmail(ApplicationUser user)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var urlEncodedCode = HttpUtility.UrlEncode(code);
        var confirmationUrl = $"{Environment.GetEnvironmentVariable(Constants.ClientUrl)}/confirm-email?userId={user.Id}&code={urlEncodedCode}";

        // Load and populate the HTML email template
        var emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/EmailTemplates/ConfirmEmailTemplate.html");
        var emailBody = await File.ReadAllTextAsync(emailTemplatePath);
        emailBody = emailBody.Replace("{USER_NAME}", user.Name);
        emailBody = emailBody.Replace("{CONFIRMATION_URL}", confirmationUrl);
        emailBody = emailBody.Replace("{CURRENT_YEAR}", DateTime.Now.Year.ToString());

        // Send the confirmation email
        await emailSender.SendEmailAsync(user.Email!, "Confirm Your Email Address", emailBody);
    }
}
