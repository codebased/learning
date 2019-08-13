# Manage Resources Group and Resources

## Login to azure through az cli

az login

## View current account and tenant id (guid)

az account show

## Get a list of providers

> A list of resources that are available for you to use

az provider list --output tsv

## Get a list of your current resources

az group list --output table

## Get a group name details

az group show --name *resource group name*

## Create a new group

az group create --name *resource group name* --location eastus

## Delete delete group

az group delete --name *resource group name*

## Task: Move existing resource to a new group

group show --name *resource name*

az resource move --ids *resource name* -destination-group *resource group name*

## Get list of resources within a group

az resource list --resource-group "resource group name"
