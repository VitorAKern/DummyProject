using Newtonsoft.Json;

namespace projectTest.Domain.Models
{
    public class Dummy
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
    }
}
