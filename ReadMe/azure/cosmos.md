<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Introduction](#introduction)
  - [Create a SQL API Cosmos DB account with session consistency and multi-master enabled](#create-a-sql-api-cosmos-db-account-with-session-consistency-and-multi-master-enabled)
  - [Create a database](#create-a-database)
  - [List account keys](#list-account-keys)
  - [List account connection strings](#list-account-connection-strings)
  - [Clean up](#clean-up)
  - [Consistency level](#consistency-level)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Introduction

AzureIt is a global distributed database service that is designed to provide:

* Low latency,
* elastic scalability
* data consistency
* high availability

declare resourceGroupName = "cosmosdb"

declare location = "westus"

declare accountName= "laaz203cosmosdb"

declare databaseName = "myDatabase"

az group create `
 -n $resourceGroupName `
 -l $location

## Create a SQL API Cosmos DB account with session consistency and multi-master enabled

az cosmosdb create `
 -g $resourceGroupName `
 --name $accountName `
 --kind GlobalDocumentDB `
 --locations "West US=0" "North Central US=1" `
 --default-consistency-level Strong `
 --enable-multiple-write-locations true `
 --enable-automatic-failover true

## Create a database

az cosmosdb database create `
 -g $resourceGroupName `
 --name $accountName `
 --db-name $databaseName

## List account keys

az cosmosdb list-keys `
 --name $accountName `
 -g $resourceGroupName

## List account connection strings

az cosmosdb list-connection-strings `
 --name $accountName `
 -g $resourceGroupName

az cosmosdb show `
 --name $accountName `
 -g $resourceGroupName `
 --query "documentEndpoint"

## Clean up

az group delete -y -g $resourceGroupName

## Consistency level

CAP theorem: you can only have 2 of consistency, availability and partition tolerance

**Consistency**: Every read receives the most recent write or an error.

**Avaiability**: Every request receives a (non-error) response - without the guarantee that it contains the most recent write

**Partition tolerence**: The system continues to operate despite an arbibrary number of messages being dropped (or delayed) by the network between nodes.

For high availability and consistency you will find in RDBS
If you want availability and partition tolerance then you will use CouchDB, Cassandra
If you want consistency and partition tolerance you will use MongoDB, HBase, ad BigTable.

To maintain the consistency the database will ensure that if there is a write request pending the read will not happen till all write is done. But it will add some slowness, the stronger it is, latency will increase.
To control consistency the system will use timestap for each write request.

In case of availability, there is no timestamp and thus we don't know at which order the read or write will happen. It will bring the system consistent but may be some read has been performed in a wrong order.

The partition, where system is always running even when one node is not alive.

 Consistency Level        | Overview              | CAP     | Uses     |
| ------------- |:-------------:| -----:|-----:|
| Strong        | All writes are read immediately by anyone. Everyone see the same thing. Similar to existing RDMS. | C: HIGHEST, A:LOWEST, P: LOWEST, Latency: High (bad), Throughput: Lowest|Financial, inventory, scheduling|
| Bounded Staleness     | Trades off lag for ensuring reads return the most recent write. Lag can be specified in time or number of operations.      |C: Consistent to a bound, A:LOW, P: LOW, Latency: High, Throughput: Low |App showing status, tracking,scores, tickers, etc.|
| Session | Default consistency in Cosmos Db. All reads on the same session (connection) are consistent.  So if you write something you will read your write. But may be not another person write.    |   C: Strong for the session, A: High, P: Moderate, Latency: Moderate, Throughput: Moderate|Social apps, fitness apps, shopping cart|
| Consistent Prefix | Bounded staleness without lag/delay. You will read consistent data  but it may be n older version      |    C: LoW, A:HIGH, P: LOW  Latency: Low, Throughput: Moderate|Social media (comments, likes), apps with updates like scores|
| Eventual Consistency | Highest availabilty and performance, but no guarantee that a rea within any specific time, for anyone, sees the latest data,. But it will eventually be consistent - no loss due to igh availability.      | C: LOWEST, A:HIGHEST, P: HIGHEST, Latency: Low Throughput: Highest|Non-ordered updates like reviews and ratings, aggregated status|

References:

<https://blog.jeremylikness.com/blog/2018-03-23_getting-behind-the-9ball-cosmosdb-consistency-levels/>

<https://docs.microsoft.com/en-us/azure/cosmos-db/sql-query-getting-started>

<https://en.wikipedia.org/wiki/CAP_theorem>

<https://en.wikipedia.org/wiki/PACELC_theorem>

[RU Calculator](https://cosmos.azure.com/capacitycalculator/)
