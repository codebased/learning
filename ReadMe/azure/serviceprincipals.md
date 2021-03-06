<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Azure Service Principal](#azure-service-principal)
  - [Scripts](#scripts)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Azure Service Principal

It is also called Managed Service Identity (MSI). The purpose of this service is to create an identity for your resources, like a user, and control what it can access, what it cannot.

## Scripts

```ps

$spname = "LaAz203SampleWebAppPrincipal"

$sp = az ad sp create-for-rbac --name $spname | ConvertFrom-Json
$sp
az ad sp delete --id $sp.appId

# See what this service principal store.

az ad sp show --id $sp.appId
az ad sp list --display-name $spname

az role assignment list --assignee $sp.appId

az role definition list --output json `
  --query '[].{"roleName":roleName, "description":description}'
az role definition list --custom-role-only false --output json --query '[].{"roleName":roleName, "description":description, "roleType":roleType}'
az role definition list --name "Contributor"
az role definition list --name "Contributor" `
  --output json --query '[].{"actions":permissions[0].actions, "notActions":permissions[0].notActions}'

$webappname = "laaz203samplewebapp"
$webapprgname = "laaz203samplewebapprg"
$webappplanname = "laaz203samplewebappplan"
$location = "westus"

az group create -n $webapprgname -l $location

az appservice plan create `
 -n $webappplanname `
 -g $webapprgname `
 --sku FREE

# create web app
az webapp create `
 -g $webapprgname `
 --plan $webappplanname `
 -n $webappname 

# make this SP able to publish to a web site
$sampleweb = az webapp show `
 --name $webappname `
 -g $webapprgname  | ConvertFrom-Json
$sampleweb.id

#Assign role to alow deployment of web code.

az role assignment create `
 --role "Website Contributor" `
 --assignee $sp.appId `
 --scope $sampleweb.id

az role assignment delete --assignee $sp.appId --role "Contributor"

# enable MSI on web app (system assigned id)
$sysid = az webapp identity assign `
 -g $webapprgname `
 -n $webappname
$sysid

az webapp identity show `
 -n $webappname -g $webapprgname

az webapp delete -n $webappname -g $webapprgname
az ad sp delete --id $sp.appId

az group delete -n $webapprgname --yes

```
