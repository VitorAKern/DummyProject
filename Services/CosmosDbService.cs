using Microsoft.Azure.Cosmos;
using projectTest.Domain.Models;
using projectTest.Services.Interfaces;

namespace projectTest.Services
{

    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Dummy item)
        {
            await this._container.CreateItemAsync<Dummy>(item, new PartitionKey(item.Id.ToString()));
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await this._container.DeleteItemAsync<Dummy>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<Dummy> GetItemAsync(Guid id)
        {
            try
            {
                ItemResponse<Dummy> response = await this._container.ReadItemAsync<Dummy>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Dummy>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Dummy>(new QueryDefinition(queryString));
            List<Dummy> results = new List<Dummy>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(Guid id, Dummy item)
        {
            await this._container.UpsertItemAsync<Dummy>(item, new PartitionKey(id.ToString()));
        }
    }
}
