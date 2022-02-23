using projectTest.Domain.Models;

namespace projectTest.Services.Interfaces
{
    public interface IDummyService
    {
        public Task<List<Dummy>> GetAllDummies();
        Task<Dummy> CreateDummyAsync(Dummy dummy);
        Task DeleteDummyAsync(Guid id);
        Task UpdateDummyAsync(Guid id, Dummy item);

    }
}
