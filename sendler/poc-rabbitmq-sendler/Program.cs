using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);
Console.WriteLine("Digite sua mensagem e aperte <ENTER>");

while (true)
{
    string message = Console.ReadLine();

    if (message == "")
        break;

    var body = Encoding.UTF8.GetBytes(message);
    var properties = new BasicProperties();
    
    await channel.BasicPublishAsync(exchange: string.Empty,
        routingKey: "hello",
        basicProperties: properties,
        body: body,
        mandatory: false,
        cancellationToken: CancellationToken.None);
    
    Console.WriteLine($" [x] Enviado {message}");
}
