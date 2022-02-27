using AutoMapper;
using SharedDummy.Domain.DSO;
using SharedDummy.Domain.Models;

namespace SharedDummy.Domain
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
