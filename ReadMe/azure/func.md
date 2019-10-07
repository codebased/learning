# Introduction

Azure functions are an implementaiton of serverless computing that allows the running of code on-demand. Functions are event-drive and shortlived and provide for automatic scalability to meet demand.

## Setup Script

$acct = "laaz203functionssa"
$rg = "functions"
$queue = "incoming-orders"

az group create -n $rg -l westus

az storage account create `
 -n $acct `
 -g $rg `
 -l westus `
 --sku Standard_LRS `
 --kind StorageV2 `
 --access-tier Hot

Get storage account key

`$key = $(az storage account keys list --account-name $acct -g $rg --query "[0].value" --output tsv)`

Create a queue 

`az storage queue create -n $queue --account-name $acct --account-key $key`

az storage account show-connection-string `
 -n $acct `
 --query "connectionString"

Submit the new message into the queue manually.

`$messagejson = Get-Content -Path message.json`

`az storage message put --account-name $acct--account-key $key --queue-name $queue --content $messagejson`

Delete the group.

`az group delete -n $rg -y`

## Code

```C#

[FunctionName("ProcessMessage")]
public static void ProcessMessage(
    /*storageAccountConnectionString key value is defined in local.settings.json*/
    [QueueTrigger("incoming-messages", Connection = "storageAccountConnectionString")]
    CloudQueueMessage queueItemInput,
    [Table("Messages", Connection = "storageAccountConnectionString")]
    ICollector<Message> tableBindingsOutput,
    ILogger log)
{
    log.LogInformation($"Processing Order (mesage Id): {queueItem.Id}");
    log.LogInformation($"Processing at: {DateTime.UtcNow}");
    log.LogInformation($"Queue Insertion Time: {queueItem.InsertionTime}");
    log.LogInformation($"Queue Expiration Time: {queueItem.ExpirationTime}");
    log.LogInformation($"Data: {queueItem.AsString}");
    tableBindings.Add(JsonConvert.DeserializeObject<Message>(
        queueItem.AsString));
}

```

### Exception Handling

If a queue triggered function throws an exception, the Azure functions runtime will capture the exception and will retry calling the function 5 times (default value) (including the first call).

If those all fail, then the runtime wil put the message in a queue named **originalqueuename**poison.

```C#
[FunctionName("ProcessMessage-Poison")]
    public static void ProcessFailedMessage(
        [QueueTrigger("incoming-messages-poison", 
                        Connection = "AzureWebJobsStorage")]
        CloudQueueMessage queueItem, 
        ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {queueItem}");
        log.LogInformation($"Data: {queueItem.AsString}");
    }
```

## Concurrency/ scaling

* The Azure Functions runtime will receive up to 16 messages and run functions for each in parallel.
* When the # being processed gets down to 8, the runtime gets another batch of 16 and processes those.
* Any VM processing messages in the function app will only process a maximum of 24 parallel messages.
  
## References

[Work with Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)

[Azure Table storage bindings for Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table)

[Azure Queue storage bindings for Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue)
