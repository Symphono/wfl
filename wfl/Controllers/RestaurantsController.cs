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
            await DatabaseProvider.GetDatabase().InsertRestaurantAsync(restaurant);
            return Created(restaurant.Id.ToString(), restaurant);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync([FromUri] string id, [FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(restaurant?.Name))
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
            if(!(await DatabaseProvider.GetDatabase().CheckRestaurantIdAsync(Id)))
            {
                return BadRequest();
            }
            return Ok(await DatabaseProvider.GetDatabase().GetRestaurantWithIdAsync(Id));
        }

    }
}