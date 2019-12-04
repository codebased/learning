<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Blob](#blob)
  - [Setup Blob container through az cli](#setup-blob-container-through-az-cli)
  - [Blob Operations](#blob-operations)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Blob

## Setup Blob container through az cli

$rg = "blobs"

$location = "westus"

$acct = "laaz203blobs"

az group create -n $rg -l $location

az storage account create `
 -g $rg `
 -n $acct `
 -l $location `
 --sku Standard_LRS

az storage account show-connection-string `
 -n $acct `
 --query "connectionString"

## Blob Operations

* Create a storage account
* Get Cloud Blob Client
* Get Container
* Set Permissions
* Get Blob Reference
* Upload
* Lost Blobs
* Download
* Lease

It is a pessimistic locking on a blob, while you perform updates and stopping others to perform any actions by someone else.

Blob and concurrency pattern

Pessimistic
Optimistic throgh eTAG

<https://azure.microsoft.com/en-au/blog/managing-concurrency-in-microsoft-azure-storage-2/>

<https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet>