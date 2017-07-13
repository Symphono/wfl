using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Symphono.Wfl.Models
{
    public class FoodOrder: IEntity
    {
        [BsonIgnoreIfNull]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string RestaurantId { get; set; }
    }
}