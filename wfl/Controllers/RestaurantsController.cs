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
            if (string.IsNullOrEmpty(restaurant?.Name))
            {
                return BadRequest();
            }
            await DatabaseProvider.GetDatabase().InsertRestaurantAsync(restaurant);
            return Created(restaurant.Id.ToString(), restaurant);
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            return Ok(await DatabaseProvider.GetDatabase().GetAllRestaurantsAsync());
        }

    }
}