﻿using System.Text;
using RabbitMQ.Client;

namespace RackApi.User;

public class RabbitMQProducer
{
    public void PublishMessage(string message)
    {
        // Replace "localhost" with the IP or hostname of your RabbitMQ instance if it's running on a different machine.
        var factory = new ConnectionFactory { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("logs", ExchangeType.Fanout);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("delete",
                string.Empty,
                null,
                body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}