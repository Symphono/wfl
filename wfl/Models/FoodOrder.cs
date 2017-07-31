using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;
using MongoDB.Bson;

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
                for(int i = 0; i < MenuSelections.Count; i++)
                {
                    MenuSelections[i].FoodOrder = this;
                }
            }
        }
        public void setStatus(EntityStatus.Status status)
        {
            Status = status;
        }
        public void addMenuSelection(MenuSelection selection)
        {
            selection.Id = ObjectId.GenerateNewId().ToString();
            if (MenuSelections == null)
            {
                MenuSelections = (new[] { selection });
            }
            else
            {
                MenuSelections.Add(selection);
            }
        }
    }
}