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
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurantAsync([FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(restaurant?.Name) || (restaurant.MenuLink != null && !Uri.IsWellFormedUriString(restaurant.MenuLink.ToString(), UriKind.Absolute)))
            {
                return BadRequest();
            }
            Restaurant r = await DatabaseProvider.GetDatabase().InsertRestaurantAsync(restaurant);
            return Created(r.Id.ToString(), r);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync([FromUri] string id, [FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(restaurant?.Name) || (restaurant.MenuLink != null && !Uri.IsWellFormedUriString(restaurant.MenuLink.ToString(), UriKind.Absolute)))
            {
                return BadRequest();
            }
            return Ok(await DatabaseProvider.GetDatabase().UpdateRestaurantAsync(id, restaurant));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DatabaseProvider.GetDatabase().GetAllRestaurantsAsync());
        }

        [Route("{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetByIdAsync([FromUri] string Id)
        {
            return Ok(await DatabaseProvider.GetDatabase().GetRestaurantWithIdAsync(Id));
        }

    }
}