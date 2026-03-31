using BaseRMS.DTOs;
using BaseRMS.Entities;

namespace BaseRMS.Services.Interfaces;

public interface IMfaService
{
    Task<bool> IsMfaEnable(ApplicationUser user);
    Task<MfaSetupDto> GenerateMfaSetupAsync(ApplicationUser user);
    Task<(bool enabled, List<string>? recoveryCodes)> VerifyAndEnableMfaAsync(ApplicationUser user, MfaVerifyRequest request);
    Task DisableMfaAsync(ApplicationUser userRequest, string emailToDisable);
    Task VerifyMfaCodeAsync(ApplicationUser user, MfaVerifyRequest request);
    Task<List<string>> GenerateRecoveryCodesAsync(ApplicationUser user);
}
