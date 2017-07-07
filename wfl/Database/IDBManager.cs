using System.Collections.Generic;
using Symphono.Wfl.Models;
using System.Threading.Tasks;

namespace Symphono.Wfl.Database
{
    public interface IDBManager
    {
        Task<Restaurant> InsertRestaurantAsync(RestaurantDto r);
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant> GetRestaurantWithIdAsync(string Id);
        Task<Restaurant> UpdateRestaurantAsync(string id, RestaurantDto restaurant);
        Task<IEnumerable<FoodOrder>> GetAllFoodOrdersAsync();
        Task<FoodOrder> InsertFoodOrderAsync(FoodOrderDto order);
    }
}
