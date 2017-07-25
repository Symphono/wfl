using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace Symphono.Wfl.Models
{
    public class MenuSelection
    {
        public int Index { get; set; }
        public string OrdererName { get; set; }
        public string Description { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public FoodOrder FoodOrder { get; set; }
    }
}