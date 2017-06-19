using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wfl.Models
{
    public class Restaurant
    {
        public static IList<Restaurant> Restaurants;
        private static int greatestID;
        private string name;
        private int id;

        static Restaurant()
        {
            Restaurants = new List<Restaurant>();
            greatestID = -1;
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int ID {
            get { return id; }
            set { id = value; }
        }

        public static int NextID()
        {
            greatestID += 1;
            return greatestID;
        }
    }
}