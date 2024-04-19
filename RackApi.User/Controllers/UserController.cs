﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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
    private readonly ApiDbContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(IConfiguration configuration, ILogger<UserController> logger, ApiDbContext context)
    {
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Login(string email, string password)
    {
        var queryable = _context.Users.AsNoTracking();

        var reslut = await queryable.Where(x => x.Email == email && x.Password == password).FirstAsync();

        if (reslut == null) return NotFound();

        var tokenString = generateJwtToken(reslut.Id);
        return tokenString;
    }

    [HttpPost]
    public async Task<ActionResult<UserModel>> Register(UserModel user)
    {
        user.CreatedAt = DateTime.Now;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var tokenString = generateJwtToken(user.Id);
        return CreatedAtAction("Login", new { email = user.Email, password = user.Password }, tokenString);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<string>> Delete(int id)
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        string token = null;

        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            token = authorizationHeader.Substring("Bearer ".Length).Trim();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (id != Convert.ToInt16(userId)) return Unauthorized();

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

        return NotFound();
    }

    private string generateJwtToken(int id)
    {
        // Generate JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["ConnectionStrings:DefaultJWTKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(8), // Token expiration time
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "http://localhost:5114",
            Issuer = "http://localhost:5012"
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}