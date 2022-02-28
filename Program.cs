using Microsoft.Azure.ServiceBus;
using projectTest.Repository;
using projectTest.Repository.Interfaces;
using projectTest.Services;
using projectTest.Services.Interfaces;
using projectTest.Services.ServiceBus;
using System.Configuration;




var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

#region Cosmos Configuration
//Initiate Cosmos


IConfigurationSection configurationSection = builder.Configuration.GetSection("CosmosDb");

string databaseName = configurationSection.GetSection("DatabaseName").Value;
string containerName = configurationSection.GetSection("ContainerName").Value;
string account = configurationSection.GetSection("Account").Value;
string key = configurationSection.GetSection("Key").Value;
Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
DummyRepository cosmosDbService = new DummyRepository(client, databaseName, containerName);
Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");


builder.Services.AddSingleton<IDummyRepository>(cosmosDbService);
#endregion


builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IQueueService, QueueService>();
builder.Services.AddHostedService<ConsumerService>();
builder.Services.AddSingleton<IQueueClient>(x => new QueueClient("Endpoint=sb://vkservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=n2ikDfj1a9z/rInbn+QDgJozXzvhm/w5YzAJS1jNBQ4=", "vkqueue"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDummyService, DummyService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
