// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("******** RabbitMQ Producer APP ***********");

// get runtime cosumner binding key from the consumer instance id 
var instanceID = "87CP";
var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();

using var channel = connection.CreateModel();
// define the exchange 
channel.ExchangeDeclare(exchange: "mytopicexchange", type: ExchangeType.Topic);


while (true)
{
    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
    var message = $" time :  {DateTime.Now.ToLongTimeString()}";
    var encodedMessage = Encoding.UTF8.GetBytes(message);
    //binding key pattern shall be like the running consumer 
    channel.BasicPublish(exchange: "mytopicexchange", routingKey: $"{instanceID}.BindingKey", null, encodedMessage);

    Console.WriteLine($"Producer publish {message}");
};

