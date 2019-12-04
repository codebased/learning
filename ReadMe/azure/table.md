<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Storage](#storage)
  - [How to create a storage account](#how-to-create-a-storage-account)
  - [Development](#development)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Storage

## How to create a storage account

Declare variables

`declare rg`

`rg="tables"`

`declare location`

`location="westus"`

`declare storage`

`storage="accounts"`

This will create a resource group

`az group create -n $rg -l $location`

This will create a storage account

`az storage account create -g $rg -l $location -n $storage --sku Standard_LRS`

This will create a connection string

`az storage account show-connection-string -n $storage --query "connectionString"`

This will delete a resource group

`az group delete -n $rg`

## Development

These are the steps that needs to be followed by the developer:

* Install the following packages:

  * Microsoft.Azure.CosmosDB.Table
  * Microsoft.Azure.Storage.Common
  * WindowsAzure.Storage

* Define table entity

```C#
    class Employee : TableEntity
    {
         public Employee() {}
        public string Name { get; set; }
    }
```

* Get Storage Account

```C#
 var storageAccount = CloudStorageAccount.Parse(connectionString);

```

* Get Cloud Table Client

```C#
 var tableClient = storageAccount.CreateCloudTableClient();
```

* Get Table Reference

```C#
var employeesTable = tableClient.GetTableReference("employees");
            await employeesTable.CreateIfNotExistsAsync();

```

* Insert / Insert Batch / Get Object by Primary Key/ or Row Key. Query Entity or Delete Entity.

```C#

// Insert
  var employee = new Employee { Name = "Amit", PartitionKey  = "Name", RowKey = Guid.NewGuid().ToString() };
            var insertOperation = TableOperation.Insert(employee);
            await employeesTable.ExecuteAsync(insertOperation);

// Insert bulk
  var batchOperation = new TableBatchOperation();
            foreach (var entity in entities)
            batchOperation.Insert(entity);
            await table.ExecuteBatchAsync(batchOperation);

// Get
   var retrieve = TableOperation.Retrieve<Accont>(pk, rk);
            var result = await table.ExecuteAsync(retrieve);
            return (T)result.Result;

// Delete
 var retrieve = TableOperation.Delete(entity);
            await table.ExecuteAsync(retrieve);

// Filter
var filterCondition = TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, name);
            var query = new TableQuery<Account>().Where(filterCondition);
            var results = await table.ExecuteQuerySegmentedAsync(query, null); // used to be ExecuteQuery / ExecuteQueryAsync
            return results.ToList();

// Delete Batch

var gamers = new [] {
                await GetAsync<Account>(table, "US", "mike@game.net"),
                await GetAsync<Account>(table, "US", "mike@contoso.net"),
                await GetAsync<Account>(table, "France", "bleu@game.net")
            }.ToList();
            gamers.ForEach(async account =>
            {
                if (account != null)
                    await DeleteAsync(table, account);
            });
```
