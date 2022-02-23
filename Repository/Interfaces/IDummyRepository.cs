using projectTest.Domain.Models;

namespace projectTest.Repository.Interfaces
{
    public interface IDummyRepository
    {
        Task<List<Dummy>> GetDummyAsync(string query);
        Task<Dummy> GetDummyAsync(Guid id);
        Task AddDummyAsync(Dummy item);
        Task UpdateDummyAsync(Guid id, Dummy item);
        Task DeleteDummyAsync(Guid id);
    }
}
