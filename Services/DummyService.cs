using projectTest.Domain.Models;
using projectTest.Repository.Interfaces;
using projectTest.Services.Interfaces;

namespace projectTest.Services
{
    public class DummyService : IDummyService
    {
        private readonly IDummyRepository _dummyRepo;

        public DummyService(IDummyRepository dummyRepo)
        {
            _dummyRepo = dummyRepo;
        }

        public List<Dummy> GetAllDummies() => _dummyRepo.GetAllDummies();
    }
}

