using Symphono.Wfl.Database;
using System.Threading.Tasks;

namespace Symphono.Wfl.Models
{
    public class FoodOrderDto
    {
        public string RestaurantId { get; set; }

        public async Task<bool> CanCreateFoodOrderAsync(IDBManager<Restaurant> restaurantDBManager)
        {
            if (string.IsNullOrEmpty(RestaurantId) || ! await IsRestaurantIdValidAsync(restaurantDBManager))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> IsRestaurantIdValidAsync(IDBManager<Restaurant> restaurantDBManager)
        {
            Restaurant r = await restaurantDBManager.GetEntityByIdAsync(RestaurantId);
            if (r != null)
            {
                System.Diagnostics.Debug.WriteLine(r.Name);
                return true;
            }
            return false;
        }
    }
}