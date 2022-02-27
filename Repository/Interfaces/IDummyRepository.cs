using Microsoft.AspNetCore.JsonPatch;
using SharedDummy.Domain.DSO;

namespace projectTest.Repository.Interfaces
{
    public interface IDummyRepository
    {
        Task<List<DummyDso>> GetDummyAsync(string query);
        Task<DummyDso> GetDummyAsync(Guid id);
        Task<DummyDso> AddDummyAsync(DummyDso item);
        Task<bool> UpdateDummyAsync(Guid id, JsonPatchDocument item);
        Task DeleteDummyAsync(Guid id);
    }
}
