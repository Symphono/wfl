using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Symphono.Wfl.Models
{
    public class RestaurantCollection
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}