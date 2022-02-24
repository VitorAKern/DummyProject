using Microsoft.AspNetCore.JsonPatch;
using projectTest.Domain.Models;

namespace projectTest.Services.Interfaces
{
    public interface IDummyService
    {
        public Task<List<Dummy>> GetAllDummies();
        Task<Dummy> CreateDummyAsync(Dummy dummy);
        Task<bool> DeleteDummyAsync(Guid id);
        Task UpdateDummyAsync(Guid id, JsonPatchDocument item);

    }
}
