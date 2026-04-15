using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RaoCattles.BuildingBlocks.Authentication;
using RaoCattles.Modules.Users.Application.Contracts;
using RaoCattles.Modules.Users.Domain.Entities;

namespace RaoCattles.Modules.Users.Infrastructure.Services;

public class JwtTokenService(JwtSettings settings) : IJwtTokenService
{
    public string GenerateToken(AdminUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
