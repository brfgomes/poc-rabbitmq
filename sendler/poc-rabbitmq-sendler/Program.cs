using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

var exchangeName = "my_exchange_test";
var queueName = "my_queue_test";
var routingKey = "my_routing_key_test";

await channel.ExchangeDeclareAsync(
    exchange: exchangeName,
    type: ExchangeType.Direct,
    durable: true,
    autoDelete: false,
    arguments: null);

await channel.QueueDeclareAsync(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

await channel.QueueBindAsync(
    queue: queueName,
    exchange: exchangeName,
    routingKey: routingKey);

Console.WriteLine("Digite sua mensagem e aperte <ENTER>");

while (true)
{
    string message = Console.ReadLine();
    if (message == "")
        break;
    var body = Encoding.UTF8.GetBytes(message);
    var properties = new BasicProperties();
    await channel.BasicPublishAsync(
        exchange: exchangeName,
        routingKey: routingKey,
        basicProperties: properties,
        body: body,
        mandatory: false,
        cancellationToken: CancellationToken.None);
    Console.WriteLine($" [x] Enviado {message}");
}
