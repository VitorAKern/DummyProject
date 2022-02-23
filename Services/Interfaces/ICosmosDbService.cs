using projectTest.Domain.Models;

namespace projectTest.Services.Interfaces
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Dummy>> GetItemsAsync(string query);
        Task<Dummy> GetItemAsync(Guid id);
        Task AddItemAsync(Dummy item);
        Task UpdateItemAsync(Guid id, Dummy item);
        Task DeleteItemAsync(Guid id);
    }
}
