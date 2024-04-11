using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    
    // [HttpGet("{id}")]
    // public async Task<ActionResult<MessageModel>> GetMessage(int id)
    // {
    //     var message = await _context.Messages.FindAsync(id);
    //
    //     if (message == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return message;
    // }

    [HttpPost]
    public async Task<ActionResult<MessageModel>> PostMessage(MessageModel message)
    {
        message.CreatedAt = DateTime.Now;

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMessages", new { id = message.Id }, message);

        var producer = new RabbitMQProducer();
        string stingJson = JsonConvert.SerializeObject(message);
        producer.PublishMessage(stingJson);
        
        return message;
    }
}