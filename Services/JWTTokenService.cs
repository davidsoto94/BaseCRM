using BaseCRM.Configurations;
using BaseCRM.Enums;
using BaseCRM.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BaseCRM.Services;

public class JWTTokenService (
    UserManager<IdentityUser> userManager,
    RoleRepository roleRepository
    )
{
    public async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var allPermissions = (await roleRepository.GetApplicationRoles()
            .Where(w => w.Name != null && roles.Contains(w.Name))
            .Select(s => s.Permitions)
            .ToListAsync())
            .SelectMany(s => s)
            .Distinct()
            .Select(p => Enum.GetName(typeof(PermissionEnum), p))
            .ToList();

        var claims = new[]
         {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("Permissions",JsonSerializer.Serialize(allPermissions))
        };

        var randomKey = Guid.NewGuid().ToString();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Constants.JwtKey) ?? randomKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: Environment.GetEnvironmentVariable(Constants.JwtIssuer),
            audience: Environment.GetEnvironmentVariable(Constants.JwtAudience),
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}