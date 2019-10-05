# Introduction 

* If you are using Linux , you must specify which runtime stack. Windows does not have this contraint.
  
* The app service plan and location OS much match.
  
| Dev/ Test | Type | Second Header |
|-- | ------------- | ------------- |
| | F1  | Shared infrastructure, no deployment slots, no custom domain no scalling  |
| | D1 | Shared infrastructure, no deployment slots, custom domains, no scaling  |
| | B1 | Dedicated infrastrcture, no deployment slots, custom domains/ SSL, manual scaling  |

| Production | Type | Second Header |
|-- | ------------- | ------------- |
| | S1/P1v2/P2v2/P3v3  | Dedicated, Auto-scale, Staging slots, custom domains/ SSL |

## Commands

### Using GITHUB

declare $rg = "webapps"

declare $planname = "githubdeployasp"

declare $appname = "laaz203githubdeploy"

declare $repourl = "https://github.com/Azure-Samples/php-docs-hello-world"

Create a group

`az group create -n $rg -l westus`

Create a app service plan

`az appservice plan create -n $planname -g $rg --sku FREE`

Create a app service

`az webapp create -n $appname -g $rg --plan $planname`

Configue github deployment

`az webapp deployment source config -n $appname -g $rg --repo-url $repourl --branch master --manual-integration`

`az webapp deployment source show -n $appname -g $rg`

It will get the webapp details

`az webapp show -n $appname -g $rg`

`az webapp show -n $appname -g $rg --query "defaultHostName" -o tsv`

Update the code and sync back to app service

`az webapp deployment source sync -n $appname -g $rg`

Delete group

`az group delete -n $rg --yes`

### Using DOCKER

Create a resource group > Create App service plan > Create WEb App > Configure github Deployment > Opne website

$rg = "webapps"

$planname = "dockerhubdeployasp"

$appname = "laaz203dockerhubdeploy"

$container = "microsoft/dotnet-samples:aspnetapp"

Create a group

`az group create -n $rg -l westus`

Create a service plan

`az appservice plan create -n $planname -g $rg --sku B1 --is-linux`

Download container and run through in app service.

`az webapp create -n $appname -g $rg --plan $planname  --deployment-container-image-name $container`

Set the port mapping for the docker container.

`az webapp config appsettings set -g $rg -n $appname --settings WEBSITES_PORT=80`

It will get the webapp details

`az webapp show -n $appname -g $rg`

`az webapp show -n $appname -g $rg --query "defaultHostName" -o tsv`

Delete group

`az group delete -n $rg --yes`

References

<https://docs.microsoft.com/en-us/azure/app-service/scripts/cli-continuous-deployment-github>

<https://azure.microsoft.com/en-au/services/app-service/containers/>

<https://docs.microsoft.com/en-us/azure/app-service/containers/quickstart-docker>
