using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Symphono.Wfl.Models
{
    public class MenuSelection
    {
        public int Index { get; set; }
        public string OrdererName { get; set; }
        public string Description { get; set; }
        [BsonIgnore]
        public FoodOrder FoodOrder { get; set; }
    }
}