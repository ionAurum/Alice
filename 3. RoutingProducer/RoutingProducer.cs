using System;
using System.Text;
using RabbitMQ.Client;

namespace RoutingProducer
{
    internal class RoutingProducer
    {
        private static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var routingKeys = args;

            var random = new Random();

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("routing", "direct");

                    var i = 0;
                    while (true)
                    {
                        var routingKey = routingKeys[random.Next(routingKeys.Length)];
                        var message = string.Format("{0}\t{1}", i++, routingKey);

                        channel.BasicPublish("routing", routingKey, null, Encoding.UTF8.GetBytes(message));

                        Console.WriteLine(message);
                    }
                }
            }
        }
    }
}