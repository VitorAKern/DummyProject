using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using projectTest.Domain;
using projectTest.Domain.DSO;
using projectTest.Domain.Models;
using projectTest.Services.Interfaces;

namespace projectTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DummyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDummyService _dummyService;

        public DummyController(IMapper mapper, IDummyService dummyService)
        {
            _mapper = mapper;
            _dummyService = dummyService;
        }


        //GET: api/<controller>  
        [HttpGet]
        public Task<List<Dummy>> GetAll()
        {
            return _dummyService.GetAllDummies();
        }

        [HttpPost]
        public Task<Dummy> Post(DummyDto request)
        {
            return _dummyService.CreateDummyAsync(_mapper.Map<DummyDso>(request));
        }

        [HttpPatch]
        public Task Patch(Guid id, DummyDto request)
        {
            return _dummyService.UpdateDummyAsync(id, _mapper.Map<DummyDso>(request));
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _dummyService.DeleteDummyAsync(id);
        }
    }
}

