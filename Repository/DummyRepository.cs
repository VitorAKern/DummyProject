using projectTest.Domain.Models;
using projectTest.Repository.Interfaces;

namespace projectTest.Repository
{
    public class DummyRepository : IDummyRepository
    {
        public List<Dummy> GetAllDummies()
        {
            List<Dummy> dummies = new List<Dummy>()
            { 
                new Dummy() 
                {
                    Id = new Guid(),
                    Name = "Dummy 1",
                    Age = 25
                },
                 new Dummy()
                {
                    Id = new Guid(),
                    Name = "Dummy 2",
                    Age = 27
                }
            };
           return dummies;
        }
    }
}