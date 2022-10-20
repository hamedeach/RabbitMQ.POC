// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("******** RabbitMQ Producer APP ***********");

var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

// for pub/ sub the producer shall not declare any queue each consumer shall declare its queue
// declare the exchange where the producer send its messages
//fan out exchange use to broadcasting
channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

while (true)
{
    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
    var message = $" time :  {DateTime.Now.ToLongTimeString()}";
    var encodedMessage = Encoding.UTF8.GetBytes(message);
    //send the defined exchange as a param
    channel.BasicPublish(exchange:"pubsub", "", null, encodedMessage);

    Console.WriteLine($"Producer publish {message}");
};

