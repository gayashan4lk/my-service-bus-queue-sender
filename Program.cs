using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("settings.json", optional:false, reloadOnChange: true)
    .Build();

var SERVICE_BUS_NAMESPACE_CONNECTION_STRING = configuration["AppSettings:SERVICE_BUS_NAMESPACE_CONNECTION_STRING"];
var SERVICE_BUS_QUEUE_NAME = configuration["AppSettings:SERVICE_BUS_QUEUE_NAME"];

Console.WriteLine($"SERVICE_BUS_NAMESPACE_CONNECTION_STRING:{SERVICE_BUS_NAMESPACE_CONNECTION_STRING}");
Console.WriteLine($"SERVICE_BUS_QUEUE_NAME:{SERVICE_BUS_QUEUE_NAME}");
