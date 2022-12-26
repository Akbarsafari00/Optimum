using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Optimum.Authentication.Jwt.Contracts;
using Optimum.Authentication.Jwt.Options;

namespace Optimum.Authentication.Jwt;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GenerateToken(IEnumerable<Claim> claims, DateTime expireDate)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_jwtOptions.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expireDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateToken(string id, string username,string name, DateTime expireDate)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_jwtOptions.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>()
            {
                new Claim("Id",id),
                new Claim("Username",username),
                new Claim("Name",name),
            }),
            Expires = expireDate,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}