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
            channel.QueueDeclare(queue: "my_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: "my_queue",
                basicProperties: null,
                body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}