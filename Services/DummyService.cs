using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Polly;
using projectTest.Repository.Interfaces;
using projectTest.Services.Interfaces;
using projectTest.Services.ServiceBus;
using SharedDummy.Domain;
using SharedDummy.Domain.DSO;
using SharedDummy.Domain.Models;
using System.Text.Json;

namespace projectTest.Services
{
    public class DummyService : IDummyService
    {
        private readonly IDummyRepository _dummyRepo;
        private readonly IMapper _mapper;
        private readonly IQueueService _queue;

        public DummyService(IDummyRepository dummyRepo, IMapper mapper, IQueueService queue)
        {
            _dummyRepo = dummyRepo;
            _mapper = mapper;
            _queue = queue;
        }

        public async Task<Dummy> CreateDummyAsync(Dummy dummy)
        {
            try
            {
                if (dummy.Id == Guid.Empty)
                    dummy.Id = Guid.NewGuid();

                var maxRetryAttempts = 3;
                var pauseBetweenFailures = TimeSpan.FromSeconds(2);
                Dummy response = new Dummy();

                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);

                await retryPolicy.ExecuteAsync(async () =>
                {
                await _dummyRepo.AddDummyAsync(_mapper.Map<DummyDso>(dummy));

                ServiceBusModel sbm = new ServiceBusModel {
                    Method = Method.POST,
                    DummyDso = _mapper.Map<DummyDso>(dummy),
                };

                    await _queue.SendMessageAsync(sbm, "vkqueue");
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
                    response = _mapper.Map<List<DummyDso>, List<Dummy>>(await _dummyRepo.GetDummyAsync("SELECT * FROM Dummy"));
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

                    ServiceBusModel sbm = new ServiceBusModel
                    {
                        Method = Method.DELETE,
                        DummyDso = new DummyDso { Id = id},
                    };

                    await _queue.SendMessageAsync(sbm, "vkqueue");
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
                    await _dummyRepo.UpdateDummyAsync(id, item);

                    ServiceBusModel sbm = new ServiceBusModel
                    {
                        Method = Method.PATCH,
                        JsonPatchItem = new JsonPatchUtil { ops = item.Operations },
                        DummyDso = new DummyDso { Id=id},   
                    };

                    await _queue.SendMessageAsync(sbm, "vkqueue");
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

