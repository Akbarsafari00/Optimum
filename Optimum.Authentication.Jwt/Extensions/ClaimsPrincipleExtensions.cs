using System.Security.Claims;

namespace Optimum.Authentication.Jwt.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string? GetId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(x => x.Type == "Id")?.Value;
    }
    
    public static string? GetUsername(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(x => x.Type == "Username")?.Value;
    }
    
    public static string? GetName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(x => x.Type == "Name")?.Value;
    }
}