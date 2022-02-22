using projectTest.Domain.Models;

namespace projectTest.Repository.Interfaces
{
    public interface IDummyRepository
    {
        List<Dummy> GetAllDummies();
    }
}
