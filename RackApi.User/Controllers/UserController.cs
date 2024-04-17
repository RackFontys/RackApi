using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RackApi.User.Data;
using RackApi.User.Models;

namespace RackApi.User.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserController> _logger;
    private readonly ApiDbContext _context;

    public UserController(IConfiguration configuration, ILogger<UserController> logger, ApiDbContext context)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "Login")]
    public async Task<ActionResult<string>> Login(string email, string password)
    {
        var queryable = _context.Users.AsNoTracking();

        var reslut = await queryable.Where(x => x.Email == email && x.Password == password).FirstAsync();

        if (reslut == null)
        { 
            return NotFound();
        }
        
        var tokenString = generateJwtToken(reslut.Email);
        return tokenString;
    }

    [HttpPost(Name = "UpdateUser")]
    public async Task<ActionResult<UserModel>> Register(UserModel user)
    {
        user.CreatedAt = DateTime.Now;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var tokenString = generateJwtToken(user.Email);
        return CreatedAtAction("Login", new { email = user.Email, password = user.Password }, user + " "+ tokenString);
    }

    [HttpDelete(Name = "DeleteUser")]
    public async Task<ActionResult<string>> Delete(int id)
    {
        var userToDelete = await _context.Users.FindAsync(id);
        if (userToDelete != null)
        {
            var producer = new RabbitMQProducer();
            producer.PublishMessage(id.ToString());
            
            _context.Users.Attach(userToDelete);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    private string generateJwtToken(string email)
    {
        // Generate JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["ConnectionStrings:DefaultJWTKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, email),
                // Add additional claims as needed
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "http://localhost:5114",
            Issuer = "http://localhost:5012"
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}