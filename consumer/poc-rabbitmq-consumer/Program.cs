using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);
    
    Console.WriteLine(" [*] Agurdando mensagens.");
    
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        
        Console.WriteLine($" [x] Recebido: {message}");
    };
    await channel.BasicConsumeAsync(queue: "hello",
        autoAck: true,
        consumer: consumer);
        
        Console.WriteLine("Aperte [enter] para sair");
        Console.ReadLine();