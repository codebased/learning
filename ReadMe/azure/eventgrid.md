# Introduction

It is fully managed event routing service that provides for event consumption using a publish-subscribe model.

Event Grid is more on events, where as Service bus is more on commands.

It takes event from an Event Sources , put this into  topics and route it to event subscriptions that is then handled by Event handler.

```ps

$rgname = "eventgrid"
$location = "westus"
$stgacctname = "laaz203egsa"

// create Group
az group create -n $rgname -l $location

az storage account create `
 -n $stgacctname `
 -g $rgname `
 -l $location `
 --sku Standard_LRS `
 --kind StorageV2

 $stgacctkey = $(az storage account keys list `
 -g $rgName `
 --account-name $stgacctname `
  --query "[0].value" `
  --output tsv)
 $stgacctkey

# run local web app and this in another terminal window
ngrok http -host-header=localhost 5000

$funcappdns = "209f0a2d.ngrok.io"
$viewerendpoint = "https://$funcappdns/api/updates"

$storageid = $(az storage account show `
 -n $stgacctname `
 -g $rgname `
 --query id `
 --output tsv)
$storageid

az eventgrid event-subscription create `
 --source-resource-id $storageid `
 --name storagesubscription `
 --endpoint-type WebHook `
 --endpoint $viewerendpoint `
 --included-event-types "Microsoft.Storage.BlobCreated" `
 --subject-begins-with "/blobServices/default/containers/testcontainer/"

az storage container create `
 --account-name $stgacctname `
 --account-key $stgacctkey `
 --name testcontainer

touch testfile.txt
az storage blob upload `
 --account-name $stgacctname `
 --account-key $stgacctkey `
 --file testfile.txt `
 --container-name testcontainer  `
 --name testfile.txt
  
az storage blob delete `
 --account-name $stgacctname `
 --account-key $stgacctkey `
 --container-name testcontainer  `
 --name testfile.txt
  
az eventgrid event-subscription delete `
 --resource-id $storageid `
 --name storagesubscription

az group delete -n $rgname --yes


```

Reference:

[Event Grid](https://azure.microsoft.com/en-us/services/event-grid/)

[What is Azure Event Grid?](https://docs.microsoft.com/en-us/azure/event-grid/overview)

[Choose between Azure messaging services - Event Grid, Event Hubs, and Service Bus](https://docs.microsoft.com/en-us/azure/event-grid/compare-messaging-services)

[Azure CLI samples for Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/cli-samples)

[Ractive Manifesto](https://ngrok.com/download)

[The Twelve-Factor App](https://12factor.net)
