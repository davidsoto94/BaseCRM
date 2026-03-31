using BaseRMS.Entities;

namespace BaseRMS.Services.Interfaces;

public interface IDeviceTrustService
{
    string GenerateDeviceFingerprint();
    string? ExtractDeviceName();
    Task<bool> IsDeviceTrusted(ApplicationUser user, string deviceFingerprint);
    TrustedDevice AddTrustedDevice(ApplicationUser user, string deviceFingerprint, string? deviceName);
    bool RemoveTrustedDevice(ApplicationUser user, int deviceId);

}
