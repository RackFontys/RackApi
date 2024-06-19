using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RackApi.Chat.Data;
using RackApi.Chat.Models;

namespace RackApi.Chat.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ILogger<MessageController> logger, ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<MessageModel>>> GetMessages(int userId)
    {
        if (userId != GetUserIdFromJWT(HttpContext)) return Unauthorized();

        var query = _context.Messages.AsNoTracking();

        var report = await query.Where(x => x.ToUserId == userId || x.UserId == userId).ToListAsync();

        return report;
    }

    [HttpPost]
    public async Task<ActionResult<MessageModel>> PostMessage(MessageModel message)
    {
        if (message.UserId != GetUserIdFromJWT(HttpContext)) return Unauthorized();

        message.CreatedAt = DateTime.Now;

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMessages", new { id = message.Id }, message);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteMessage(int userId)
    {
        var messagesToDelete =
            await _context.Messages.Where(x => x.ToUserId == userId || x.UserId == userId).ToListAsync();
        
        var isNotEmpty = messagesToDelete.Any();
        if (isNotEmpty)
        {
            foreach (var message in messagesToDelete)
            {
                _context.Messages.Attach(message);
                _context.Messages.Remove(message);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        return NotFound();
    }

    private int? GetUserIdFromJWT(HttpContext httpContext)
    {
        var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        string token = null;

        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            token = authorizationHeader.Substring("Bearer ".Length).Trim();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        return Convert.ToInt16(userId);
    }
}