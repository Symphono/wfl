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
            Restaurant r = new Restaurant()
            {
                Name = restaurant.Name,
                MenuLink = restaurant.MenuLink
            };
            r = await DBManager.InsertEntityAsync(r);
            return Created(r.Id.ToString(), r);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync([FromUri] string id, [FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(id) || await DBManager.GetEntityWithIdAsync<Restaurant>(id) == null || string.IsNullOrEmpty(restaurant?.Name) || (restaurant.MenuLink != null && !Uri.IsWellFormedUriString(restaurant.MenuLink.ToString(), UriKind.Absolute)))
            {
                return BadRequest();
            }
            Restaurant r = new Restaurant()
            {
                Name = restaurant.Name,
                MenuLink = restaurant.MenuLink
            };
            return Ok(await DBManager.UpdateEntityAsync(id, r));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DBManager.GetAllEntitiesAsync<Restaurant>());
        }

        [Route("{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string id)
        {
            return Ok(await DBManager.GetEntityWithIdAsync<Restaurant>(id));
        }

    }
}