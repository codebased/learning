using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Learning.TableAPI
{
    public class Playground : Controller
    {
        private readonly IConfiguration _configuration;

        public Playground(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [Route("/tables")]
        [HttpGet]
        public async Task<ActionResult<CustomerEntity>> Start()
        {
            var table = await CreateTableAsync("customers");
            await InsertOrMergeEntityAsync(table, new CustomerEntity("malhotra", "amit") { Email = "codebased@hotmail.com", PhoneNumber = "9239472" });
            return await RetrieveEntityUsingPointQueryAsync(table, "malhotra", "amit");
        }

        private async Task<CustomerEntity> InsertOrMergeEntityAsync(CloudTable table, CustomerEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            // Create the InsertOrReplace table operation
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

            // Execute the operation.
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            CustomerEntity insertedCustomer = result.Result as CustomerEntity;

            return insertedCustomer;
        }

        private async Task<CustomerEntity> RetrieveEntityUsingPointQueryAsync(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(partitionKey, rowKey);
            TableResult result = await table.ExecuteAsync(retrieveOperation);
            CustomerEntity customer = result.Result as CustomerEntity;

            return customer;
        }

        private async Task<CustomerEntity> DeleteEntityAsync(CloudTable table, CustomerEntity deleteEntity)
        {
            if (deleteEntity == null)
            {
                throw new ArgumentNullException("deleteEntity");
            }

            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
            TableResult result = await table.ExecuteAsync(deleteOperation);
            return result.Result as CustomerEntity;

        }

        private async Task QueryAsync(CloudTable table, string lastName, string email)
        {
            TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>()
                .Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, lastName),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email)
                ));

            await table.ExecuteQuerySegmentedAsync(query, null);
        }

        private async Task<CloudTable> CreateTableAsync(string tableName)
        {
            string storageConnectionString = _configuration["StorageConnectionString"];

            // Retrieve storage account information from connection string.
            var storageAccount = Common.CreateStorageAccountFromConnectionString(storageConnectionString);

            // Create a table client for interacting with the table service
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            Console.WriteLine("Create a Table for the demo");

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Created Table named: {0}", tableName);
            }
            else
            {
                Console.WriteLine("Table {0} already exists", tableName);
            }

            Console.WriteLine();
            return table;
        }
    }
}