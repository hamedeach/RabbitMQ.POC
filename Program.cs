// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("******** RabbitMQ Server APP ***********");

var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

//declare the request queue in our server in case it starts before the client
var responseQueue = channel.QueueDeclare(queue: "request-queue", exclusive: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += Consumer_Received;

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    var properties = channel.CreateBasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;

    Console.WriteLine($" the server Received a request id : {e.BasicProperties.CorrelationId}");
    var replaymessage = $" the time is  :  {DateTime.Now.ToLongTimeString()}";
    var encodedMessage = Encoding.UTF8.GetBytes(replaymessage);
    channel.BasicPublish("", e.BasicProperties.ReplyTo, properties, encodedMessage);
}

channel.BasicConsume(queue: "request-queue", autoAck: true, consumer: consumer);


Console.ReadLine();

//while (true)
//{
//    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
//    var message = $" time :  {DateTime.Now.ToLongTimeString()}";
//    var encodedMessage = Encoding.UTF8.GetBytes(message);
//    channel.BasicPublish("", "echoQ", null, encodedMessage);

//    Console.WriteLine($"Producer publish {message}");
//};

