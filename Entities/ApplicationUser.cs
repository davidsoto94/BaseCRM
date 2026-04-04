using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace BaseRMS.Entities;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    [JsonIgnore]
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public List<TrustedDevice> TrustedDevices { get; set; } = new List<TrustedDevice>();
}
