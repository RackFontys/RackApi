namespace RackApi.Chat;
using System.Text;
using RabbitMQ.Client;

public class RabbitMQProducer
{
    public void PublishMessage(string message)
    {
        // Replace "localhost" with the IP or hostname of your RabbitMQ instance if it's running on a different machine.
        var factory = new ConnectionFactory() { HostName = "localhost" }; 

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "messages",
                routingKey: string.Empty,
                basicProperties: null,
                body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}