using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Symphono.Wfl.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using System;

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
        public static bool CanDtoStatusConvertToEnum(FoodOrderStatusDto dto)
        {
            StatusOptions status;
            if(Enum.TryParse(dto.Status, false, out status))
            {
                return true;
            }
            return false;
        }

        public static async Task<bool> CanConstructFromDtoAsync(FoodOrderDto dto, IDBManager<Restaurant> restaurantDBManager)
        {
            if (dto == null || string.IsNullOrEmpty(dto.RestaurantId) || !await IsRestaurantIdValidAsync(dto.RestaurantId, restaurantDBManager))
            {
                return false;
            }
            return true;
        }

        private static async Task<bool> IsRestaurantIdValidAsync(string RestaurantId, IDBManager<Restaurant> restaurantDBManager)
        {
            Restaurant r = await restaurantDBManager.GetEntityByIdAsync(RestaurantId);
            if (r != null)
            {
                System.Diagnostics.Debug.WriteLine(r.Name);
                return true;
            }
            return false;
        }

        public bool CanSetStatus(FoodOrderStatusDto dto)
        {
            if(dto != null && CanDtoStatusConvertToEnum(dto))
            {
                StatusOptions dtoStatus = (StatusOptions)Enum.Parse(typeof(StatusOptions), dto.Status);
                if (dtoStatus == StatusOptions.Active)
                {
                    return CanReactivate();
                }
                else if (dtoStatus == StatusOptions.Discarded)
                {
                    return CanDiscard();
                }
                else if (dtoStatus == StatusOptions.Completed)
                {
                    return CanComplete();
                }
            }
            return false;
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

    public class StatusOptionsRepresentation
    {
        public IList<string> Values { get; set; }
    }
}