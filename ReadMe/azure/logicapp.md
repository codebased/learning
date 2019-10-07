# Introduction

It is as same as Biztalk in cloud. It implements event-driven, serverless, potentially long-running, workflows. While function is code first, Logic Apps are declarative.

## Exception Handling

| Type | Description |
|-- | ------------- | ------------- |
| Default | This Policy sends up to four retries at exponentially increasing intevals, which scale by 7.5 seconds but are capped between 5 and 45 seconds. |
| Exponential Interval | This policy waits a random interval selected from an exponentially growing range before sending the next request |
| Fixed Interval | This policy waits the specified interval before sending the next request.|
| None | Don't resend the request. |

### References

[Overview - What is Azure Logic Apps?](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-overview)

[Overview: Azure serverless with Azure Logic Apps and Azure Functions](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-serverless-overview)

[Handle errors and exceptions in Azure Logic Apps
](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-exception-handling)

[What are Microsoft Flow, Logic Apps, Functions, and WebJobs?
](https://docs.microsoft.com/en-us/azure/azure-functions/functions-compare-logic-apps-ms-flow-webjobs)
