using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http;
using System.Web.Http.Results;
using wfl.Models;


namespace wfl.Controllers
{
    [RoutePrefix("api/Restaurant")]
    public class RestaurantsController : ApiController
    {
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult CreateRestaurant([FromUri] string name)
        {
            if (name == null || name.Equals(""))
            {
                return BadRequest();
            }
            Restaurant restaurant = new Restaurant()
            {
                Name = name,
                ID = Restaurant.NextID()
            };
            Restaurant.Restaurants.Add(restaurant);
            return Created(name, restaurant);
        }

        [Route("")]
        [HttpGet]
        [ResponseType(typeof(IList<Restaurant>))]
        public IHttpActionResult Get()
        {
            return Ok(Restaurant.Restaurants);
        }

    }
}