using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<Dummy> Post(DummyDto request)
        {
            var response = await _dummyService.CreateDummyAsync(_mapper.Map<Dummy>(request));
            return response;
        }

        [HttpPatch("{id}")]
        public async Task Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument request)
        {
            if (request != null)
            {
                await _dummyService.UpdateDummyAsync(id, request);
            }
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _dummyService.DeleteDummyAsync(id);
        }
    }
}

