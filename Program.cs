// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("********RabbitMQ Producer APP***********");

var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "echoQ",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

while (true)
{
    Thread.Sleep(1000);
    var message = $"OF A7 Gamed at {DateTime.Now.ToLongTimeString()}";
    var encodedMessage = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish("", "echoQ", null, encodedMessage);

    Console.WriteLine($"Producer publish {message}");
};

