using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RackApi.Chat.Controllers;

namespace RackApi.Chat;

public class RabbitMQConsumer
{
    private readonly ILogger<RabbitMQConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly IConfiguration _configuration;

    public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
    }

    public async Task ConsumeMessages()
    {
        _logger.LogInformation("Waiting for RabbitMQ to be ready...");

        // Adding a delay of 10 seconds to wait for RabbitMQ to be ready
        await Task.Delay(TimeSpan.FromSeconds(30));

        _logger.LogInformation("Trying to connect to RabbitMQ...");
        
        var rabbitMqHost = _configuration["DatabaseStrings:RABBITMQ_HOST"];
        int rabbitMqPort = Convert.ToInt16(_configuration["DatabaseStrings:RABBITMQ_PORT"]);
        var rabbitMqUsername = _configuration["DatabaseStrings:RABBITMQ_USERNAME"];
        var rabbitMqPassword = _configuration["DatabaseStrings:RABBITMQ_PASSWORD"];
        
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqHost,
            UserName = rabbitMqUsername,
            Password = rabbitMqPassword,
            Port = rabbitMqPort,
            RequestedHeartbeat = TimeSpan.FromSeconds(30),
        };

        try
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("delete", ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "delete", string.Empty);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {message}", message);

                await ProcessMessage(message);
            };

            channel.BasicConsume(queueName, true, consumer);

            _logger.LogInformation("Consumer is waiting for messages.");
            await Task.Delay(-1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to RabbitMQ.");
            throw;
        }
    }

    private async Task ProcessMessage(string message)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var messageController = scope.ServiceProvider.GetRequiredService<MessageController>();
        await messageController.DeleteMessage(Convert.ToInt16(message));
    }
}