using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Symphono.Wfl.Models
{
    public class FoodOrder: IContainerEntity
    {
        public enum StatusOptions
        {
            Active = 1,
            Discarded,
            Completed
        }
        [BsonIgnoreIfNull]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string RestaurantId { get; set; }
        public IList<MenuSelection> MenuSelections { get; set; }
        public StatusOptions Status { get; private set; } = StatusOptions.Active;
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
        public void setStatus(StatusOptions status)
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