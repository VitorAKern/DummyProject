using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Polly;
using projectTest.Domain.DSO;
using projectTest.Domain.Models;
using projectTest.Repository.Interfaces;
using projectTest.Services.Interfaces;
using System.Text.Json;

namespace projectTest.Services
{
    public class DummyService : IDummyService
    {
        private readonly IDummyRepository _dummyRepo;
        private readonly IMapper _mapper;

        public DummyService(IDummyRepository dummyRepo, IMapper mapper)
        {
            _dummyRepo = dummyRepo;
            _mapper = mapper;
        }

        public async Task<Dummy> CreateDummyAsync(Dummy dummy)
        {
            try
            {
                var maxRetryAttempts = 3;
                var pauseBetweenFailures = TimeSpan.FromSeconds(2);
                Dummy response = new Dummy();

                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

                await retryPolicy.ExecuteAsync(async () =>
                {
                    return await _dummyRepo.AddDummyAsync(_mapper.Map<DummyDso>(dummy));
                });

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Dummy GetDummiesById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Dummy>> GetAllDummies()
        {
            try
            {
                var maxRetryAttempts = 3;
                var pauseBetweenFailures = TimeSpan.FromSeconds(2);
                List<Dummy> response = new List<Dummy>();

                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

                await retryPolicy.ExecuteAsync(async () =>
                {
                    response = _mapper.Map<List<DummyDso>, List<Dummy>>(_dummyRepo.GetDummyAsync("SELECT * FROM Dummy").GetAwaiter().GetResult());
                   // mapper.Map<Source[], List<Destination>>(sources);
                    return response;
                });

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteDummyAsync(Guid id)
        {
            try
            {
                var maxRetryAttempts = 3;
                var pauseBetweenFailures = TimeSpan.FromSeconds(2);


                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

                await retryPolicy.ExecuteAsync(async () =>
                {
                    _dummyRepo.DeleteDummyAsync(id).GetAwaiter();
                });
                return true;
            }
            catch { }
            return false;
        }

        public async Task UpdateDummyAsync(Guid id, JsonPatchDocument item)
        {
            try
            {
                var maxRetryAttempts = 3;
                var pauseBetweenFailures = TimeSpan.FromSeconds(2);


                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

                await retryPolicy.ExecuteAsync(async () =>
                {
                    return await _dummyRepo.UpdateDummyAsync(id, item);
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

