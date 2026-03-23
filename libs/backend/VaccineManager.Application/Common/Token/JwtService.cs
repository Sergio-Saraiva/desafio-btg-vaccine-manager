using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VaccineManager.Application.Common.Settings;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace VaccineManager.Application.Common.Token;

public class JwtService : ITokenService
{
    private readonly AppSettings _appSettings;

    public JwtService(IOptions<AppSettings> options)
    {
        _appSettings = options.Value;
    }
    public string GenerateToken(string email)
    {
        var secretKey = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Key);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(secretKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_appSettings.JwtSettings.ExpirationMinutes));

        var token = new JwtSecurityToken(
            issuer: _appSettings.JwtSettings.Issuer,
            audience: _appSettings.JwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}