using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using projectTest.Services.Interfaces;
using SharedDummy.Domain;
using SharedDummy.Domain.Models;

namespace projectTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DummyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDummyService _dummyService;
        private IMemoryCache _memoryCache;

        public DummyController(IMapper mapper, IDummyService dummyService, IMemoryCache cache)
        {
            _mapper = mapper;
            _dummyService = dummyService;
            _memoryCache = cache;
        }


        //GET: api/<controller>  
        [HttpGet]
        public async Task<List<Dummy>> GetAll()
        {
            List<Dummy> dummies = new List<Dummy>();

            if(!_memoryCache.TryGetValue("GETALL", out dummies))
            {
                dummies = await _dummyService.GetAllDummies();
                var cacheEntryOptions = new MemoryCacheEntryOptions().
                    SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _memoryCache.Set("GETALL", dummies, cacheEntryOptions);
                    
            }

            return dummies;
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

