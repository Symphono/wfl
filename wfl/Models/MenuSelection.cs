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
        public static async Task<bool> CanConstructFromDtoAsync(MenuSelectionDto dto, string foodOrderId, IDBManager<FoodOrder> foodOrderDBManager)
        {
            if (dto == null || string.IsNullOrEmpty(foodOrderId) || string.IsNullOrEmpty(dto.OrdererName) || string.IsNullOrEmpty(dto.Description) || !await IsFoodOrderValidAsync(foodOrderId, foodOrderDBManager))
            {
                return false;
            }
            return true;
        }
        private static async Task<bool> IsFoodOrderValidAsync(string foodOrderId, IDBManager<FoodOrder> foodOrderDBManager)
        {
            FoodOrder order = await foodOrderDBManager.GetEntityByIdAsync(foodOrderId);
            if (order?.Status != FoodOrder.StatusOptions.Active)
            {
                return false;
            }
            return true;
        }
    }
}