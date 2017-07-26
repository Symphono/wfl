using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Symphono.Wfl.Models
{
    public class FoodOrder: IContainerEntity
    {
        [BsonIgnoreIfNull]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string RestaurantId { get; set; }
        public IList<MenuSelection> MenuSelections { get; set; }
        public EntityStatus.Status Status { get; private set; } = EntityStatus.Status.Active;
        public void OnDeserialize()
        {
            if (MenuSelections != null)
            {
                foreach (MenuSelection selection in MenuSelections)
                {
                    selection.FoodOrder = this;
                }
            }
        }
        public void setStatus(EntityStatus.Status status)
        {
            Status = status;
        }
    }
}