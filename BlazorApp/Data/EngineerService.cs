using Microsoft.Azure.Cosmos;

namespace BlazorApp.Data
{
    public class EngineerService(IConfiguration config) : IEngineerService
    {
        private readonly IConfiguration _config = config;
        private readonly string DbName = "Contractors";
        private readonly string ContainerName = "Engineers";

        private Container GetContainerClient()
        {
            var cosmosClient = new CosmosClient(_config["CosmosDbConnectionString"]);
            return cosmosClient.GetContainer(DbName, ContainerName);
        }

        public async Task AddEngineer(Engineer engineer)
        {
            try
            {
                engineer.id = Guid.NewGuid();
                var container = GetContainerClient();
                await container.CreateItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
            }
            catch (CosmosException ex)
            {
                // Log these 3 critical values to instantly find the fix
                Console.WriteLine($"HTTP Status Code: {ex.StatusCode}");
                Console.WriteLine($"SubStatus Code: {ex.SubStatusCode}");
                Console.WriteLine($"Diagnostics: {ex.Diagnostics}");
            }
        }

        public async Task UpdateEngineer(Engineer engineer)
        {
            var container = GetContainerClient();
            await container.ReplaceItemAsync(engineer, engineer.id.ToString(), new PartitionKey(engineer.id.ToString()));
        }

        public async Task DeleteEngineer(Guid? id)
        {
            var container = GetContainerClient();
            await container.DeleteItemAsync<Engineer>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<List<Engineer>> GetEngineerDetails()
        {
            var container = GetContainerClient();
            var iterator = container.GetItemQueryIterator<Engineer>(new QueryDefinition("SELECT * FROM c"));

            var results = new List<Engineer>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<Engineer?> GetEngineerDetailsById(Guid? id)
        {
            var container = GetContainerClient();

            try
            {
                var response = await container.ReadItemAsync<Engineer>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}