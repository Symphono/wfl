using System.Threading.Tasks;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Models
{
    public class MenuSelectionDto
    {
        public string OrdererName { get; set; }
        public string Description { get; set; }
        
        public async Task<bool> CanCreateMenuSelectionAsync(IDBManager<FoodOrder> foodOrderDBManager, string foodOrderId)
        {
            if(!PropertiesHaveValues() || ! await IsFoodOrderValidAsync(foodOrderDBManager, foodOrderId))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IsFoodOrderValidAsync(IDBManager<FoodOrder> foodOrderDBManager, string foodOrderId)
        {
            FoodOrder order = await foodOrderDBManager.GetEntityByIdAsync(foodOrderId);
            if (order?.Status != FoodOrder.StatusOptions.Active)
            {
                return false;
            }
            return true;
        }

        private bool PropertiesHaveValues()
        {
            if (string.IsNullOrEmpty(OrdererName) || string.IsNullOrEmpty(Description))
            {
                return false;
            }
            return true;
        }
    }
}