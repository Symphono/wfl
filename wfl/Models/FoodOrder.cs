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
                foreach (MenuSelection selection in MenuSelections)
                {
                    selection.FoodOrder = this;
                }
            }
        }
        public void SetStatus(StatusOptions status)
        {
            Status = status;
        }
        public void AddMenuSelection(MenuSelection selection)
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
        public bool CanCreateMenuSelection()
        {
            if (Status == StatusOptions.Active)
            {
                return true;
            }
            return false;
        }
        public bool CanDiscard()
        {
            if (Status == StatusOptions.Active)
            {
                return true;
            }
            return false;
        }
        public bool CanComplete()
        {
            if(Status == StatusOptions.Active)
            {
                return true;
            }
            return false;
        }
        public bool CanReactivate()
        {
            if (Status == StatusOptions.Discarded)
            {
                return true;
            }
            return false;
        }
    }
}