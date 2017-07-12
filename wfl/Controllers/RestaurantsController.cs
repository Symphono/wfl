using System;
using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/Restaurant")]
    public class RestaurantsController : ApiController
    {
        IDBManager DBManager { get; }
        public RestaurantsController(IDBManager dbManager)
        {
            this.DBManager = dbManager;
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurantAsync([FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(restaurant?.Name) || (restaurant.MenuLink != null && !Uri.IsWellFormedUriString(restaurant.MenuLink.ToString(), UriKind.Absolute)))
            {
                return BadRequest();
            }
            Restaurant r = await DBManager.InsertRestaurantAsync(restaurant);
            return Created(r.Id.ToString(), r);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync([FromBody] RestaurantDto restaurant, [FromUri] string id)
        {
            if (string.IsNullOrEmpty(id) || await DBManager.GetRestaurantWithIdAsync(id) == null || string.IsNullOrEmpty(restaurant?.Name) || (restaurant.MenuLink != null && !Uri.IsWellFormedUriString(restaurant.MenuLink.ToString(), UriKind.Absolute)))
            {
                return BadRequest();
            }
            return Ok(await DBManager.UpdateRestaurantAsync(id, restaurant));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            RestaurantCollection restaurantCollection = new RestaurantCollection()
            {
                Restaurants = await DBManager.GetAllRestaurantsAsync()
            };
            return Ok(restaurantCollection);
        }

        [Route("{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await DBManager.GetRestaurantWithIdAsync(id));
        }

    }
}