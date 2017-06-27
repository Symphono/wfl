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
            if (restaurant == null || string.IsNullOrEmpty(restaurant.Name))
            {
                return BadRequest();
            }
            await DatabaseProvider.GetDatabase().InsertRestaurantAsync(restaurant);
            return Created(restaurant.Id.ToString(), restaurant);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateNameAsync([FromUri] string id, [FromBody] RestaurantDto restaurant)
        {
            if (string.IsNullOrEmpty(id) || restaurant == null || string.IsNullOrEmpty(restaurant.Name))
            {
                return BadRequest();
            }
            return Ok(await DatabaseProvider.GetDatabase().UpdateRestaurantNameAsync(id, restaurant.Name));
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DatabaseProvider.GetDatabase().GetAllRestaurantsAsync());
        }

    }
}