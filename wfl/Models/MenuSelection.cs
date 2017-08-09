using MongoDB.Bson.Serialization.Attributes;
using System.Threading.Tasks;
using Symphono.Wfl.Database;
using Newtonsoft.Json;

namespace Symphono.Wfl.Models
{
    public class MenuSelection
    {
        public string Id { get; set; }
        public string OrdererName { get; set; }
        public string Description { get; set; }
        [BsonIgnore]
        [JsonIgnore]
        public FoodOrder FoodOrder { get; set; }
    }
}