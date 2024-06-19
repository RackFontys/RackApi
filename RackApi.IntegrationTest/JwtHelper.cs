using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RackApi.IntegrationTest;

public class JwtHelper
{
    public static string GenerateToken(int id)
    {
        var audience = "{{JsonWebTokenStrings:AudienceIp}}";
        var issuer = "{{JsonWebTokenStrings:IssuerIp}}";
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("{{JsonWebTokenStrings:DefaultJWTKey}}");
        
        Console.WriteLine("values: " + audience + ", " + issuer + ", " + key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Token expires in 1 hour, you can change this as needed
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = audience,
            Issuer = issuer
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}