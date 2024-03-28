using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RackApi.Chat.Controllers;

[ApiController]
[Route("[controller]")]

public class MessageController : ControllerBase
{
    
    
    private readonly ILogger<MessageController> _logger;

    public MessageController(ILogger<MessageController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetMessage")]
    public Message Get()
    {
        var message = new Message
        {
            Date = DateOnly.FromDateTime(DateTime.Now.Date),
            messageText = "Test message of a user",
            userId = Random.Shared.Next(0, 100),
            id = Random.Shared.Next(0, 100)
        };

        var producer = new RabbitMQProducer();
        string stingJson = JsonConvert.SerializeObject(message);
        producer.PublishMessage(stingJson);
        
        return message;
    }
}