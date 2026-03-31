namespace BaseRMS.Services.Interfaces;

public interface IEmailTemplateService
{
    Task<string> GetConfirmationEmailHtmlAsync(string userName, string confirmationUrl, int currentYear);
    Task<string> GetResetPasswordEmailHtmlAsync(string resetPasswordUrl, int currentYear);
}
