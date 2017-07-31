using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Symphono.Wfl.Models
{
    public class FoodOrderCollection
    {
        public IEnumerable<FoodOrder> FoodOrders { get; set; }
    }
}