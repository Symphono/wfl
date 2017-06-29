﻿using System.Collections.Generic;
using Symphono.Wfl.Models;
using System.Threading.Tasks;

namespace Symphono.Wfl.Database
{
    public interface IDBManager
    {
        Task InsertRestaurantAsync(RestaurantDto r);
        Task<bool> CheckRestaurantIdAsync(string id);
        Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();
        Task<RestaurantDto> GetRestaurantWithIdAsync(string Id);
    }
}
