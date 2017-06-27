using System.Collections.Generic;
using Symphono.Wfl.Models;
using System.Threading.Tasks;

namespace Symphono.Wfl.Database
{
    public interface IDBManager
    {
        Task InsertRestaurantAsync(RestaurantDto r);
        Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();
        Task<RestaurantDto> UpdateRestaurantNameAsync(string id, string name);
    }
}
