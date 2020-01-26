using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Adventure.Persistence.Containers
{
    public class Item : Resource
    {
        [JsonProperty(PropertyName = "parentId")]
        public string ParenId { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "children")]
        public Children[] Children { get; set; }
    }

    public class Children
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class Doughnut : Item
    { }
}
