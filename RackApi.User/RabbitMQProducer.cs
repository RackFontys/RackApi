using System.Text;
using RabbitMQ.Client;

namespace RackApi.User;

public class RabbitMQProducer
{
    protected readonly IConfiguration _configuration;
    
    public RabbitMQProducer(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void PublishMessage(string message)
    {
        var rabbitMqHost = _configuration["ConnectionStrings:RABBITMQ_HOST"];
        int rabbitMqPort = Convert.ToInt16(_configuration["ConnectionStrings:RABBITMQ_PORT"]);
        var rabbitMqUsername = _configuration["ConnectionStrings:RABBITMQ_USERNAME"];
        var rabbitMqPassword = _configuration["ConnectionStrings:RABBITMQ_PASSWORD"];
        
        var factory = new ConnectionFactory { HostName = rabbitMqHost, Port = rabbitMqPort, UserName = rabbitMqUsername, Password = rabbitMqPassword, };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("delete", ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("delete",
                string.Empty,
                null,
                body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}