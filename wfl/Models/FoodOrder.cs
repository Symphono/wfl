using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
namespace wfl.Models
{
    public class FoodOrder
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId ID { get; set; }
        public string RestaurantID { get; set; }
    }
}