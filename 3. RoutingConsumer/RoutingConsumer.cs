using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RoutingConsumer
{
    internal class RoutingConsumer
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var routingKey = args[0];

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("routing", "direct");

                    var queueName = channel.QueueDeclare();
                    channel.QueueBind(queueName, "routing", routingKey);

                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };

                    Console.ReadLine();
                }
            }
        }
    }
}