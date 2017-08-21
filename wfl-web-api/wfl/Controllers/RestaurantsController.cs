using System;
using System.Web.Http;
using System.Threading.Tasks;
using Symphono.Wfl.Models;
using System.Collections.Generic;
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
            if(!Restaurant.CanConstructFromDto(restaurant))
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
            Restaurant restaurantEntity = await dbManager.GetEntityByIdAsync(id);
            if (restaurantEntity == null || !Restaurant.CanConstructFromDto(restaurant))
            {
                return BadRequest();
            }

            restaurantEntity.Name = restaurant.Name;
            restaurantEntity.MenuLink = restaurant.MenuLink;
            return Ok(await dbManager.UpdateEntityAsync(id, restaurantEntity));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync([FromUri] NameSearchCriteria criteria)
        {
            IEnumerable<Restaurant> restaurants;
            if (criteria?.HasCriteria() == true)
            {
                restaurants = await dbManager.GetEntitiesAsync(criteria);
            }
            else
            {
                restaurants = await dbManager.GetEntitiesAsync(null);
            }
            RestaurantCollection restaurantCollection = new RestaurantCollection()
            {
                Restaurants = restaurants,
                Criteria = criteria
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