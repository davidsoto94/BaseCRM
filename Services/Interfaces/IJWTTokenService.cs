using BaseRMS.Entities;

namespace BaseRMS.Services.Interfaces;

public interface IJWTTokenService
{
    Task<string> GenerateJwtToken(ApplicationUser user, string? scope = null);
    RefreshToken GenerateRefreshToken(string ipAddress);
}
