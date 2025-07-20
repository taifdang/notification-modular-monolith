using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared;
using Hookpay.Shared.Caching;
using Hookpay.Shared.EFCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, string>
{
    private readonly UserDbContext _context;
    private readonly IRequestCache _cache;
    private readonly IConfiguration _configuration;
    
    public SignInCommandHandler(UserDbContext context,IRequestCache cache, IConfiguration configuration) {  _context = context;_cache = cache;_configuration = configuration; }
    public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        //Email:index non-cluster
        var user =  await _context.Users.SingleOrDefaultAsync(x => x.Email == request.email);
        if (user is null) return string.Empty;
        if (!user.Password.Equals(request.password)) return string.Empty;
        if (user is null) return string.Empty;
        var result = GenerateJwtToken(user.Id,user.Email,user.Username);
        return result;
    }
    public string GenerateJwtToken(int userId,string email,string username)
    {
        try
        {
            var claims = new[]
            {
                new Claim("uid",userId.ToString()),
                //new Claim(JwtRegisteredClaimNames.Name,Username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,username),
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a-string-secret-at-least-256-bits-long"));
            var value = _configuration.GetOptions<KeyOptions>("jwt").key;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value));
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
            throw new Exception("fail when get key");
        }
        
    }
}
