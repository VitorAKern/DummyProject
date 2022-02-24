using projectTest.Domain.Models;
using projectTest.Repository;
using projectTest.Repository.Interfaces;
using projectTest.Services;
using projectTest.Services.Interfaces;
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
builder.Services.AddHttpContextAccessor();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDummyService, DummyService>();

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
