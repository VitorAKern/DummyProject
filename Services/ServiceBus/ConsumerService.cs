using Microsoft.Azure.ServiceBus;
using SharedDummy.Domain;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using projectTest.Repository.Interfaces;

namespace projectTest.Services.ServiceBus
{
    public class ConsumerService : BackgroundService
    {
        private readonly IQueueClient _queueClient;
        private readonly IDummyRepository _repository;

        public ConsumerService(IQueueClient queueClient, IDummyRepository repository)
        {
            _queueClient = queueClient;
            _repository = repository;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _queueClient.RegisterMessageHandler(async (message, token) =>
            {

                var dummy = System.Text.Json.JsonSerializer.Deserialize<ServiceBusModel>(Encoding.UTF8.GetString(message.Body));
                if (dummy != null)
                {
                    switch (dummy.Method)
                    {
                        case Method.POST:
                            await _repository.AddDummyAsync(dummy.DummyDso!);
                            break;
                        case Method.DELETE:
                            await _repository.DeleteDummyAsync(dummy.DummyDso!.Id);
                            break;
                        case Method.PATCH:
                            var patchDocument = new JsonPatchDocument(dummy.JsonPatchItem!.ops, new Newtonsoft.Json.Serialization.DefaultContractResolver());
                            await _repository.UpdateDummyAsync(dummy.DummyDso!.Id, patchDocument!);
                            break;
                    }
                }

                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

            }, new MessageHandlerOptions(args => Task.CompletedTask)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1
            });
            return Task.CompletedTask;
        }
    }
}
