using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using projectTest.Domain;
using projectTest.Domain.Models;

namespace projectTest.Controllers
{

    [Route("api/[controller]")]
    public class DummyController : Controller
    {
        private readonly IMapper _mapper;

        public DummyController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/<controller>  
        [HttpGet]
        public Dummy Get()
        {
            DummyDto dummyDTO = new DummyDto()
            {
                Id = Guid.NewGuid(),
                Name = "Dummy 1",
                Age = 25
            };

            return _mapper.Map<Dummy>(dummyDTO);
        }
    }
}

