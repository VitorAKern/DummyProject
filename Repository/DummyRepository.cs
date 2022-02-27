using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Azure.Cosmos;
using projectTest.Repository.Interfaces;
using SharedDummy.Domain.DSO;
using SharedDummy.Domain.Models;

namespace projectTest.Repository
{
    public class DummyRepository : IDummyRepository
    {
        private Container _container;

        public DummyRepository(
            CosmosClient dbClient,
            string databaseName,
            string containerName
            )
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<DummyDso> AddDummyAsync(DummyDso item)
        {
            var response = await _container.CreateItemAsync(item, new PartitionKey(item.Id.ToString()));
            return response;    
        }

        public async Task DeleteDummyAsync(Guid id)
        {
            await this._container.DeleteItemAsync<Dummy>(id.ToString(), new PartitionKey(id.ToString()));
        }

        public async Task<DummyDso> GetDummyAsync(Guid id)
        {
            try
            {
                ItemResponse<DummyDso> response = await this._container.ReadItemAsync<DummyDso>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<List<DummyDso>> GetDummyAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<DummyDso>(new QueryDefinition(queryString));
            List<DummyDso> results = new List<DummyDso>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<bool> UpdateDummyAsync(Guid id, JsonPatchDocument item)
        {
            try
            {
                var dummy = await this._container.ReadItemAsync<DummyDso>(id.ToString(), new PartitionKey(id.ToString()));

                if (dummy != null)
                {
                    item.ApplyTo(dummy.Resource);
                    await _container.UpsertItemAsync(dummy.Resource, new PartitionKey(id.ToString()));
                }
                return true;
            }
            catch (Exception ex) {

                throw new Exception(ex.Message);
            }
        }
    }
}