using AutoMapper;
using projectTest.Domain.Models;

namespace projectTest.Domain
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DummyDto, Dummy>();
        }
    }
}
