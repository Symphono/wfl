using System;
using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using Symphono.Wfl.Database;

namespace Symphono.Wfl.Controllers
{
    [RoutePrefix("api/restaurant")]
    public class RestaurantsController : ApiController
    {
        private IDBManager<Restaurant> dbManager { get; }
        public RestaurantsController(IDBManager<Restaurant> dbManager)
        {
            this.dbManager = dbManager;
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurantAsync([FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(restaurant?.Name) || (restaurant.MenuLink != null && !Uri.IsWellFormedUriString(restaurant.MenuLink.ToString(), UriKind.Absolute)))
            {
                return BadRequest();
            }
            Restaurant r = new Restaurant()
            {
                Name = restaurant.Name,
                MenuLink = restaurant.MenuLink
            };
            r = await dbManager.InsertEntityAsync(r);
            return Created(r.Id.ToString(), r);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync([FromUri] string id, [FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(id) || await dbManager.GetEntityByIdAsync(id) == null || string.IsNullOrEmpty(restaurant?.Name) || (restaurant.MenuLink != null && !Uri.IsWellFormedUriString(restaurant.MenuLink.ToString(), UriKind.Absolute)))
            {
                return BadRequest();
            }

            Restaurant restaurantEntity = await dbManager.GetEntityByIdAsync(id);
            restaurantEntity.Name = restaurant.Name;
            restaurantEntity.MenuLink = restaurant.MenuLink;
            return Ok(await dbManager.UpdateEntityAsync(id, restaurantEntity));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            RestaurantCollection restaurantCollection = new RestaurantCollection()
            {
                Restaurants = await dbManager.GetAllEntitiesAsync()
            };
            return Ok(restaurantCollection);
        }

        [Route("{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await dbManager.GetEntityByIdAsync(id));
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync([FromUri] string id)
        {
            return Ok(await dbManager.DeleteEntityByIdAsync(id));
        }

    }
}