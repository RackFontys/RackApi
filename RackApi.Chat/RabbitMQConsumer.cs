using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RackApi.Chat.Controllers;

namespace RackApi.Chat;

public class RabbitMQConsumer
{
    private readonly ILogger<RabbitMQConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task ConsumeMessages()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("delete", ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queueName,
                "delete",
                string.Empty);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {message}", message);

                await ProcessMessage(message);
            };

            channel.BasicConsume(queueName,
                true,
                consumer);

            _logger.LogInformation("Consumer is waiting for messages.");
            await Task.Delay(-1);
        }
    }

    private async Task ProcessMessage(string message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var messageController = scope.ServiceProvider.GetRequiredService<MessageController>();
            await messageController.DeleteMessage(Convert.ToInt16(message));
        }

        await Task.CompletedTask;
    }
}