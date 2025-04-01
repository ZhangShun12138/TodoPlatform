using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TodoBackend.tools;
public class JwtHelper
{
    private readonly JwtTokenSettings _jwtSettings;

    public JwtHelper(IOptions<JwtTokenSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // 唯一标识符
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.SiteUrl,
            audience: _jwtSettings.SiteUrl,
            claims: claims,
            expires: DateTimeOffset.Now.Date.AddHours(_jwtSettings.TokenExpires), // 过期时间
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
