<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Introduction](#introduction)
  - [Important Points](#important-points)
    - [References](#references)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Introduction

## Important Points

* WebJobs is not yet supported for App Service on Linux
* A web app can time out after 20 minutes of inactivity. Only requests to the actual web app reset the timer.
* If your app runs continuous or scheduled (Timer trigger) WebJobs, enable Always On to ensure that the WebJobs run reliably. This feature is available only in the Basic, Standard, and Premium pricing tiers.

WebJob types

| Continuous | Triggered |
| -----------| ---------- |
| Starts immediately when the WebJob is created. To keep the job from ending, the program or script typically does its work inside an endless loop. If the job does end, you can restart it. | Starts only when triggered manually or on a schedule. |
|Runs on all instances that the web app runs on. You can optionally restrict the WebJob to a single instance. | Runs on a single instance that Azure selects for load balancing.|
| Supports remote debugging. | Doesn't support remote debugging. |

### References

[Run Background tasks with WebJobs in Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/web-sites-create-web-jobs)