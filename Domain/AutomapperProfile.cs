using AutoMapper;
using projectTest.Domain.DSO;
using projectTest.Domain.Models;

namespace projectTest.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DummyDto, Dummy>();
            CreateMap<Dummy, DummyDso>();
            CreateMap<DummyDso, Dummy>();
        }
    }
}
