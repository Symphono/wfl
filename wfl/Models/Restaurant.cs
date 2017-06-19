using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfl.Models
{
    public class Restaurant
    {
        public static IList<Restaurant> Restaurants;
        private string name;

        static Restaurant()
        {
            Restaurants = new List<Restaurant>();
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }
    }
}