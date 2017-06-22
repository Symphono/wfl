using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;

namespace wfl.Models
{
    public class Restaurant
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId ID { get; set; }
        public string Name { get; set; }
    }
}