using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RackApi.User;

public class RabbitMQConsumer
{
    
    public void ConsumeMessages()
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received: {0}", message);
            };

            channel.BasicConsume(queue: "my_queue",
                autoAck: true,
                consumer: consumer);

            // Keep the consumer alive by blocking here
            Console.WriteLine("Consumer is waiting for messages. Press [enter] to stop.");
            Console.ReadLine();
        }
    }
}