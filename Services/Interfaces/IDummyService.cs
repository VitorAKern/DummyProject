using projectTest.Domain.DSO;
using projectTest.Domain.Models;

namespace projectTest.Services.Interfaces
{
    public interface IDummyService
    {
        public Task<List<Dummy>> GetAllDummies();
        Task<Dummy> CreateDummyAsync(DummyDso dummy);
        Task DeleteDummyAsync(Guid id);
        Task UpdateDummyAsync(Guid id, DummyDso item);

    }
}
