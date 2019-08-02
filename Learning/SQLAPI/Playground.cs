using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Learning.SQLAPI
{
    public class Playground : Controller
    {
        public Playground(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/sql")]
        [HttpGet]
        public async Task<ActionResult<List<Family>>> Init()
        {
            return await GetStartedDemoAsync();
        }

        private CosmosClient cosmosClient;

        private Database database;

        private Container container;

        private string databaseId = "FamilyDatabase";
        private string containerId = "FamilyContainer";

        private IConfiguration _configuration;

        private async Task CreateDatabaseAsync()
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", database.Id);
        }

        private async Task CreateContainerAsync()
        {
            container = await database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
            Console.WriteLine("Created Container: {0}\n", container.Id);
        }

        private async Task AddItemsToContainerAsync()
        {
            Family andersenFamily = new Family
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new Parent[]
                {
           new Parent { FirstName = "Thomas" },
           new Parent { FirstName = "Mary Kay" }
                },
                Children = new Child[]
                {
           new Child
            {
                FirstName = "Henriette Thaulow",
                Gender = "female",
                Grade = 5,
                Pets = new Pet[]
                {
                    new Pet { GivenName = "Fluffy" }
                }
            }
                },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = false
            };

            try
            {
                ItemResponse<Family> andersenFamilyResponse = await container.ReadItemAsync<Family>(andersenFamily.Id, new PartitionKey(andersenFamily.LastName));
                Console.WriteLine("Item in database with id: {0} already exists\n", andersenFamilyResponse.Resource.Id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ItemResponse<Family> andersenFamilyResponse = await container.CreateItemAsync<Family>(andersenFamily, new PartitionKey(andersenFamily.LastName));
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", andersenFamilyResponse.Resource.Id, andersenFamilyResponse.RequestCharge);
            }
        }

        private async Task<List<Family>> QueryItemsAsync()
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Andersen'";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Family> queryResultSetIterator = container.GetItemQueryIterator<Family>(queryDefinition);

            List<Family> families = new List<Family>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Family> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Family family in currentResultSet)
                {
                    families.Add(family);
                    Console.WriteLine("\tRead {0}\n", family);
                }
            }

            return families;
        }

        private async Task DeleteDatabaseAndCleanupAsync()
        {
            DatabaseResponse databaseResourceResponse = await database.DeleteAsync();
            cosmosClient.Dispose();
        }

        private async Task<List<Family>> GetStartedDemoAsync()
        {
            // Create a new instance of the Cosmos Client
            cosmosClient = new CosmosClient(_configuration["CosmoDbStorageSQLAPConnectionString"]);
            await CreateDatabaseAsync();
            await CreateContainerAsync();
            await AddItemsToContainerAsync();
            return await QueryItemsAsync();
        }
    }
}