## App Insights


## Commands

```ps
$resourceGroup = "appinsights"
$appInsightsName = "laaz203aisample"
az group create -n $resourceGroup -l eastus

$propsFile = "props.json"
'{"Application_Type":"web"}' | Out-File $propsFile
az resource create `
    -g $resourceGroup -n $appInsightsName `
    --resource-type "Microsoft.Insights/components" `
    --properties "@$propsFile"
Remove-Item $propsFile

az resource show -g $resourceGroup -n $appInsightsName `
    --resource-type "Microsoft.Insights/components" `
    --query "properties.InstrumentationKey" -o tsv

az group delete -n $resourceGroup
```

## Code level changes

`services.AddApplicationInsightsTelemetry("<instrumentation key>");`

In order to collect telemetry information, you need to add a listner.

`Trace.Listeners.Add(new ApplicationInsightsTraceListener());`

and whatever information you want to log, you use this code

`Trace.TraceInformation("In HomeController.Index");`

You can configure your tracing to be Sampling.

There are three types of Sampling:

* Adaptive sampling (default), Adjust data sent from the SDK and your app.
* Fixed rate sampling that you set the rate, and client and server synchornize sampling rate.
* Ingestion sampling that you configure inside of the portal. It discards some data as it arrives from your app, but does not reduce trafice (but does reduce storage requirements and hence cost).

## Usage Analytics

| Feature | Primary Capability | Need |
| ------- | ------------------ | ---- |
| Funnel  | Track progression of a user through a series of steps in your app | Which pages related to create a customer ticket? |
| Impact | How do page load times and other properties influence conversion rates | How do page load time effect conversion |
| Retention | How many users return and how often do they perform particular tasks or achieve goals | How do tasks in an app effect people returning? |
| User Flows | Show how users navigate between pages and features ( repeat action) | Are there places where user repeat the same action over and over? |
---

References:

[What is Application Insights?](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)

[Application Insights for ASP.NET Core applications](https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core
)

[Usage analysis with Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/usage-overview)  Very Important

