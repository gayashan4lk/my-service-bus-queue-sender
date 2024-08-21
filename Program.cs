using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("settings.json", optional:false, reloadOnChange: true)
    .Build();

var SERVICE_BUS_NAMESPACE_CONNECTION_STRING = configuration["AppSettings:SERVICE_BUS_NAMESPACE_CONNECTION_STRING"];
var SERVICE_BUS_QUEUE_NAME = configuration["AppSettings:SERVICE_BUS_QUEUE_NAME"];

var numOfMessages = 100;

Console.WriteLine($"SERVICE_BUS_NAMESPACE_CONNECTION_STRING:{SERVICE_BUS_NAMESPACE_CONNECTION_STRING}");
Console.WriteLine($"SERVICE_BUS_QUEUE_NAME:{SERVICE_BUS_QUEUE_NAME}");

var client = new ServiceBusClient(
    SERVICE_BUS_NAMESPACE_CONNECTION_STRING, 
    new ServiceBusClientOptions()
    {
        TransportType = ServiceBusTransportType.AmqpWebSockets
    });

var sender = client.CreateSender(SERVICE_BUS_QUEUE_NAME);

using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 0; i < numOfMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Gaya service bus message {i}")))
    {
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}

try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}
