using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using projectTest.Domain;
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


        // GET: api/<controller>  
        [HttpGet]
        public List<Dummy> GetAll()
        {
            return _dummyService.GetAllDummies();
            //return _mapper.Map<Dummy>(dummyDTO);
        }
    }
}

