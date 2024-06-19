using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RackApi.IntegrationTest;

public class JwtHelper
{
    public static string GenerateToken(int id)
    {
        var audience = "http://test.localhost:5114";
        var issuer = "http://test.localhost:5012";
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("bfbaa608ebbd10c99e64855e87f874046d1db7cf5c01a3b8b52264fbe521fa1b"); // Replace "your_secret_key" with your actual secret key
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