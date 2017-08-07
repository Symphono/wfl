﻿using System;
using System.Threading.Tasks;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Models
{
    public class RestaurantDto
    {
        public string Name { get; set; }
        public Uri MenuLink { get; set; }
        public async Task<bool> CanUpdateRestaurantAsync(string id, IDBManager<Restaurant> restaurantDBManager)
        {
            if(CanCreateRestaurant() && await IsRestaurantIdValidAsync(id, restaurantDBManager))
            {
                return true;
            }
            return false;
        }
        public bool CanCreateRestaurant()
        {
            if(string.IsNullOrEmpty(Name) || (MenuLink != null && !MenuLinkIsWellFormed()))
            {
                return false;
            }
            return true;
        }
        private bool MenuLinkIsWellFormed()
        {
            if(!Uri.IsWellFormedUriString(MenuLink.ToString(), UriKind.Absolute))
            {
                return false;
            }
            return true;
        }
        private async Task<bool> IsRestaurantIdValidAsync(string id, IDBManager<Restaurant> restaurantDBManager)
        {
            Restaurant r = await restaurantDBManager.GetEntityByIdAsync(id);
            if (r != null)
            {
                return true;
            }
            return false;
        }
    }
}