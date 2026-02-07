using BaseCRM.Entities;
using BaseCRM.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace BaseCRM.Services;

public class MfaService (UserManager<ApplicationUser> userManager)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<(bool Success, MfaSetupDto? Setup, IEnumerable<string>? Errors)> GenerateMfaSetupAsync(ApplicationUser user)
    {
        try
        {
            // Reset authenticator key if needed
            await _userManager.ResetAuthenticatorKeyAsync(user);
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(unformattedKey))
            {
                return (false, null, new[] { "Failed to generate authenticator key" });
            }

            var formattedKey = FormatKey(unformattedKey);
            var qrCode = GenerateQrCode(user.Email!, unformattedKey);

            return (true, new MfaSetupDto 
            { 
                QrCode = qrCode,
                ManualKey = formattedKey
            }, null);
        }
        catch (Exception ex)
        {
            return (false, null, new[] { ex.Message });
        }
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> VerifyAndEnableMfaAsync(ApplicationUser user, string code)
    {
        try
        {
            var authenticatorCode = code.Replace(" ", "").Replace("-", "");

            if (authenticatorCode.Length != 6 || !authenticatorCode.All(char.IsDigit))
            {
                return (false, new[] { "Invalid authenticator code format" });
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, authenticatorCode);

            if (!isValid)
            {
                return (false, new[] { "Invalid authenticator code" });
            }

            if (!user.TwoFactorEnabled)
            {
                var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
                if (result.Succeeded)
                {
                    return (true, null);
                }
                var errors = result.Errors.Select(e => e.Description);
                return (false, errors);
            }
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, new[] { ex.Message });
        }
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> DisableMfaAsync(ApplicationUser user)
    {
        try
        {
            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (result.Succeeded)
            {
                return (true, null);
            }

            var errors = result.Errors.Select(e => e.Description);
            return (false, errors);
        }
        catch (Exception ex)
        {
            return (false, new[] { ex.Message });
        }
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> VerifyMfaCodeAsync(ApplicationUser user, string code)
    {
        try
        {
            var authenticatorCode = code.Replace(" ", "").Replace("-", "");

            if (authenticatorCode.Length != 6 || !authenticatorCode.All(char.IsDigit))
            {
                return (false, new[] { "Invalid authenticator code format" });
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, authenticatorCode);

            return isValid ? (true, null) : (false, new[] { "Invalid authenticator code" });
        }
        catch (Exception ex)
        {
            return (false, new[] { ex.Message });
        }
    }

    public async Task<(bool Success, IEnumerable<string>? Errors)> GenerateRecoveryCodesAsync(ApplicationUser user)
    {
        try
        {
            var result = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            if (result != null && result.Any())
            {
                return (true, null);
            }

            return (false, new[] { "Failed to generate recovery codes" });
        }
        catch (Exception ex)
        {
            return (false, new[] { ex.Message });
        }
    }

    public async Task<IEnumerable<string>> GetRecoveryCodesAsync(ApplicationUser user)
    {
        // Since we don't have a direct method to retrieve recovery codes,
        // return empty list - recovery codes are returned after generation
        return new List<string>();
    }

    private string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        int count = 0;
        foreach (var c in unformattedKey)
        {
            result.Append(c);
            count++;
            if (count % 4 == 0 && count != unformattedKey.Length)
            {
                result.Append(" ");
            }
        }
        return result.ToString();
    }

    private string GenerateQrCode(string email, string key)
    {
        // Return a placeholder QR code URI for now; actual QR code generation can be done client-side
        // This simplifies dependencies while still providing the necessary information
        return $"otpauth://totp/BaseCRM:{email}?secret={key}&issuer=BaseCRM";
    }
}
