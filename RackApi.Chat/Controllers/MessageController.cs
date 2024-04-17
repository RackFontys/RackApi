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
    private readonly ILogger<MessageController> _logger;
    private readonly ApiDbContext _context;

    public MessageController(ILogger<MessageController> logger, ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet("{userId}")]
    public async Task<List<MessageModel>> GetMessages(int userId)
    {
        var query = _context.Messages.AsNoTracking();

        var report = await query.Where(x => x.ToUserId == userId).ToListAsync();

        return report;
    }

    [HttpPost]
    public async Task<ActionResult<MessageModel>> PostMessage(MessageModel message)
    {
        message.CreatedAt = DateTime.Now;

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMessages", new { id = message.Id }, message);
    }

    [HttpDelete]
    public async Task<ActionResult<String>> DeleteMessage(int userId)
    {
        var messagesToDelete = await _context.Messages.Where(x => x.ToUserId == userId).ToListAsync();
        
        bool isNotEmpty = messagesToDelete.Any();
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
}