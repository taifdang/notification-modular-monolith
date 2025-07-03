using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Auth.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly IDistributedCache _redis;  
        private readonly IConfiguration _configuration;
        public CustomerService(IDistributedCache distributedCache,IConfiguration configuration)
        {
            _redis = distributedCache;           
            _configuration = configuration;
        }
        public void SignIn()
        {
            //claim
            
        }
        public string GenerateJwtToken(string username)
        {
            try
            {
                var claims = new[] {
                new Claim("uid",username),
                new Claim(JwtRegisteredClaimNames.Sub,username),
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),//create_at
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())//uuid
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Hostname"],
                    audience: _configuration["Hostname"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: cred
                );
                var setToken = new JwtSecurityTokenHandler().WriteToken(token);
                _redis.SetString($"token_active_{username}", setToken);
                return setToken;
            }
            catch
            {
                return null!;
            }
        }
        public void SetTokenInCache()
        {
            _redis.SetString("","");
        }
    }
}
