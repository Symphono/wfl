using System.Collections.Generic;
using Symphono.Wfl.Models;
using System.Threading.Tasks;

namespace Symphono.Wfl.Database
{
    public interface IDBManager
    {
        Task InsertRestaurantAsync(RestaurantDto r);
        Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();
        Task<RestaurantDto> UpdateRestaurantAsync(string id, RestaurantDto restaurant);
        Task<IEnumerable<FoodOrderDto>> GetAllFoodOrdersAsync();
        Task InsertFoodOrderAsync(FoodOrderDto order);
        Task<bool> CheckRestaurantIdAsync(string id);
    }
}
