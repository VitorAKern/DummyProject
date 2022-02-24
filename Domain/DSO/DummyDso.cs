using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace projectTest.Domain.DSO
{
    public class DummyDso
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
    }
}
