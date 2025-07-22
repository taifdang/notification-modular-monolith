using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hookpay.Shared.Jwt;

public static class JwtGenerate
{
    public static string GenerateJwtToken(int userId, string email, string username)
    {
        try
        {
            var claims = new []
            {
                new Claim("uid",userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,username),
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
           
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a-string-secret-at-least-256-bits-long"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                   issuer: "https://hookpay.com",
                   audience: "https://hookpay.com",
                   claims: claims,
                   expires: DateTime.UtcNow.AddMinutes(10),
                   signingCredentials: cred
               );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
        catch
        {
            throw new Exception("Token generate is invalid");
        }
    }
}
