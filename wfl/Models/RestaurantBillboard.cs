using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfl.Models
{
    public class RestaurantBillboard
    {
        private int totalRestaurants;
        private int pageNumber;
        private int pageSize;
        private IList<Restaurant> Restaurants;
    }
}