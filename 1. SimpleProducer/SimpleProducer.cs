using System;
using System.Text;
using RabbitMQ.Client;

namespace SimpleProducer
{
    internal class SimpleProducer
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
                    channel.QueueDeclare("simple-consumer", false, false, false, null);

                    long messageCount = 0;
                    while (true)
                    {
                        var message = "msg " + messageCount++;

                        channel.BasicPublish(string.Empty, "simple-consumer", null, Encoding.UTF8.GetBytes(message));

                        Console.WriteLine(message);
                    }
                }
            }
        }
    }
}