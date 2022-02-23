using Polly;
using projectTest.Domain.Models;
using projectTest.Repository.Interfaces;
using projectTest.Services.Interfaces;
using System.Text.Json;

namespace projectTest.Services
{
    public class DummyService : IDummyService
    {
        private readonly IDummyRepository _dummyRepo;

        public DummyService(IDummyRepository dummyRepo)
        {
            _dummyRepo = dummyRepo;
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
                    return _dummyRepo.AddDummyAsync(dummy);
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
                    response = await _dummyRepo.GetDummyAsync("SELECT * FROM Dummy");
                      
                    return response;
                });

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteDummyAsync(Guid id)
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
                    return _dummyRepo.DeleteDummyAsync(id).GetAwaiter();
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task UpdateDummyAsync(Guid id, Dummy item)
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
                    return _dummyRepo.UpdateDummyAsync(id, item).GetAwaiter();
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}

