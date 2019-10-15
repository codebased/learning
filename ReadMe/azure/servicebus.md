# Introduction

```ps

az group create -n servicebus -l westus

az servicebus namespace create --n laaz203sb -g servicebus

az servicebus namespace authorization-rule keys list `
 -g servicebus `
 --namespace-name laaz203sb `
 --name RootManageSharedAccessKey `
 --query primaryConnectionString

az servicebus queue create `
 --namespace-name laaz203sb `
 -g servicebus `
 -n testqueue

New-AzureRmServiceBusQueue `
 -ResourceGroupName servicebus `
 -NamespaceName laaz203sb `
 -name testqueue `
 -EnablePartitioning $false

```

```C#
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.ServiceBus;

namespace linuxacademy.az203.thirparty.servicebus
{
    class Program {
        static void Main(string[] args) {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            const string serviceBusConnectionString = 
                "Endpoint=sb://laaz203sb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=QaWhbl9Qq6JiFCSXalz3YAgeSdsoc00hAdKIxBYkvyE=";
            const string queueName = "testqueue";
            const int delay = 2000;
            const int numMessageToSend = 10;

            var queueClient = new QueueClient(
                serviceBusConnectionString, 
                queueName);

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(
                async (message, cancellationToken) => {
                    Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
                    if (delay > 0) await Task.Delay(delay);
                    await queueClient.CompleteAsync(
                        message.SystemProperties.LockToken);
                },
                new MessageHandlerOptions(
                    exception => {
                        Console.WriteLine($"Message handler encountered an exception {exception.Exception}.");
                        var context = exception.ExceptionReceivedContext;
                        Console.WriteLine($"- Endpoint: {context.Endpoint}");
                        Console.WriteLine($"- Entity Path: {context.EntityPath}");
                        Console.WriteLine($"- Executing Action: {context.Action}");
                        return Task.CompletedTask;
                    }
                )
                {
                    MaxConcurrentCalls = 5,
                    AutoComplete = false
                }
            );

            for (var i = 0; i < numMessageToSend; i++) {
                var messageBody = $"Message {i}";
                var message = new Message(
                    Encoding.UTF8.GetBytes(messageBody));

                Console.WriteLine($"Sending message: {messageBody}");

                await queueClient.SendAsync(message);
            }

            Task.Delay(30000).Wait();

            await queueClient.CloseAsync();
        }
    }
}
```

References:

[What is Azure Service Bus?](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview)

[Get started with Service Bus queues](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues)

[Quickstart: Use the Azure CLI to create a Service Bus queue](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quickstart-cli)

[Message routing and correlation](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messages-payloads?#message-routing-and-correlation)

[Quickstart: Use Azure PowerShell to create a Service Bus queue](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quickstart-powershell)
