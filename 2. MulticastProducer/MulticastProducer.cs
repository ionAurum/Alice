using System;
using System.Text;
using RabbitMQ.Client;

namespace MulticastProducer
{
    internal class MulticastProducer
    {
        private static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("multicast", "fanout");

                    long messageCount = 0;
                    while (true)
                    {
                        var message = "msg " + messageCount++;

                        channel.BasicPublish("multicast", string.Empty, null, Encoding.UTF8.GetBytes(message));

                        Console.WriteLine(message);
                    }
                }
            }
        }
    }
}