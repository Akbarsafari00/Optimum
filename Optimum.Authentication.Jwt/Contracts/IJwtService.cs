using System.Security.Claims;

namespace Optimum.Authentication.Jwt.Contracts;

public interface IJwtService
{
    string GenerateToken(IEnumerable<Claim> claims, DateTime expireDate);
    string GenerateToken(string id, string username,string name, DateTime expireDate);
}