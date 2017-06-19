using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http;
using wfl.Models;


namespace wfl.Controllers
{
    [RoutePrefix("api/Restaurant")]
    public class RestaurantsController : ApiController
    {
        [Route("")]
        [HttpPost]
        public Restaurant CreateRestaurant([FromUri] string name)
        {
            Restaurant restaurant = new Restaurant()
            {
                Name = name,
                ID = Restaurant.NextID()
            };
            Restaurant.Restaurants.Add(restaurant);
            return restaurant;
        }

        [Route("")]
        [HttpGet]
        public IList<Restaurant> Get()
        {
            return Restaurant.Restaurants;
        }

    }
}