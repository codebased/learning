<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Manage Resources Group and Resources](#manage-resources-group-and-resources)
  - [Login to azure through az cli](#login-to-azure-through-az-cli)
  - [View current account and tenant id (guid)](#view-current-account-and-tenant-id-guid)
  - [Get a list of providers](#get-a-list-of-providers)
  - [Get a list of your current resources](#get-a-list-of-your-current-resources)
  - [Get a group name details](#get-a-group-name-details)
  - [Create a new group](#create-a-new-group)
  - [Delete delete group](#delete-delete-group)
  - [Task: Move existing resource to a new group](#task-move-existing-resource-to-a-new-group)
  - [Get list of resources within a group](#get-list-of-resources-within-a-group)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

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
