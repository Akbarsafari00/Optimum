using System.ComponentModel;

namespace Optimum.Authentication.Jwt.Options;

public class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}